using Dominio.Abstracciones;
using Dominio.Libros;
using Dominio.Prestamos.Events;

namespace Dominio.Prestamos;

/// <summary>
/// Entidad que representa un préstamo de un libro a un usuario.
/// Gestiona el ciclo de vida de un préstamo desde su creación hasta su devolución.
/// Coordina con la entidad Libro para asegurar que los estados sean coherentes.
/// </summary>
public sealed class Prestamo : Entidad
{
    private Prestamo(
        Guid libroId,
        Guid usuarioId,
        FechasPrestamo fechas,
        EstadoPrestamo estado,
        DateTime creadoEnUtc
    )
    {
        LibroId = libroId;
        UsuarioId = usuarioId;
        Fechas = fechas;
        Estado = estado;
        CreadoEnUtc = creadoEnUtc;
    }

    /// <summary>
    /// Identificador del libro siendo prestado.
    /// </summary>
    public Guid LibroId { get; private set; }

    /// <summary>
    /// Identificador del usuario que realiza el préstamo.
    /// </summary>
    public Guid UsuarioId { get; private set; }

    /// <summary>
    /// Fechas del préstamo (fecha de inicio y opcionalmente fecha de devolución).
    /// </summary>
    public FechasPrestamo Fechas { get; private set; }

    /// <summary>
    /// Estado actual del préstamo (Activo, Devuelto, Vencido).
    /// </summary>
    public EstadoPrestamo Estado { get; private set; }

    /// <summary>
    /// Fecha y hora UTC en que el préstamo fue creado.
    /// </summary>
    public DateTime CreadoEnUtc { get; private set; }

    /// <summary>
    /// Registra un nuevo préstamo de un libro a un usuario.
    /// Valida la disponibilidad del libro y actualiza su estado.
    /// </summary>
    /// <param name="libro">El libro a prestar.</param>
    /// <param name="usuarioId">Identificador del usuario que realiza el préstamo.</param>
    /// <param name="utcNow">Fecha y hora actual UTC.</param>
    /// <returns>Resultado con el préstamo creado, o un error si el libro no está disponible.</returns>
    public static Resultado<Prestamo> Registrar(
        Libro libro,
        Guid usuarioId,
        DateTime utcNow
    )
    {
        var resultadoPrestamo = libro.PrestarLibro(utcNow);
        if (resultadoPrestamo.EsFallo)
        {
            return Resultado.Fallo<Prestamo>(resultadoPrestamo.Error);
        }

        var fechas = new FechasPrestamo(utcNow, null);

        var prestamo = new Prestamo(
            libro.Id,
            usuarioId,
            fechas,
            EstadoPrestamo.Activo,
            utcNow
        );

        prestamo.RegistrarEventoDominio(new PrestamoCreadoEventoDominio(prestamo.Id, libro.Id, usuarioId));

        return Resultado.Exito(prestamo);
    }

    /// <summary>
    /// Registra la devolución de un libro prestado.
    /// Valida que el préstamo esté activo y actualiza los estados de préstamo y libro.
    /// </summary>
    /// <param name="libro">El libro siendo devuelto.</param>
    /// <param name="utcNow">Fecha y hora actual UTC.</param>
    /// <returns>Resultado indicando éxito o error si el préstamo no está activo.</returns>
    public Resultado RegistrarDevolucion(Libro libro, DateTime utcNow)
    {
        if (Estado != EstadoPrestamo.Activo)
        {
            return Resultado.Fallo(ErroresPrestamo.NoActivo);
        }

        var resultadoDevolucion = libro.RegistrarDevolucion(utcNow);
        if (resultadoDevolucion.EsFallo)
        {
            return Resultado.Fallo(resultadoDevolucion.Error);
        }

        Estado = EstadoPrestamo.Devuelto;
        Fechas = new FechasPrestamo(Fechas.FechaPrestamoUtc, utcNow);

        RegistrarEventoDominio(new DevolucionRegistradaEventoDominio(Id, LibroId, UsuarioId));

        return Resultado.Exito();
    }
}
