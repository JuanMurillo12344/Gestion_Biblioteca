using Dominio.Abstracciones;
using Dominio.Libros.Events;

namespace Dominio.Libros;

/// <summary>
/// Entidad que representa un libro en la biblioteca.
/// Un libro es la unidad base del dominio de la biblioteca. Posee información como título, ISBN y autor.
/// Maneja su propia disponibilidad y puede ser prestado o devuelto.
/// </summary>
public sealed class Libro : Entidad
{
    private Libro(
        Titulo titulo,
        Isbn isbn,
        Autor autor,
        EstadoDisponibilidad estado,
        DateTime creadoEnUtc
    )
    {
        Titulo = titulo;
        Isbn = isbn;
        Autor = autor;
        Estado = estado;
        CreadoEnUtc = creadoEnUtc;
    }

    /// <summary>
    /// Título del libro.
    /// </summary>
    public Titulo Titulo { get; private set; }

    /// <summary>
    /// ISBN del libro (identificador internacional único).
    /// </summary>
    public Isbn Isbn { get; private set; }

    /// <summary>
    /// Autor del libro.
    /// </summary>
    public Autor Autor { get; private set; }

    /// <summary>
    /// Estado actual de disponibilidad del libro (Disponible, Prestado, NoDisponible).
    /// </summary>
    public EstadoDisponibilidad Estado { get; private set; }

    /// <summary>
    /// Fecha y hora UTC en que el libro fue creado en el sistema.
    /// </summary>
    public DateTime CreadoEnUtc { get; private set; }

    /// <summary>
    /// Fecha y hora UTC del último préstamo realizado sobre este libro.
    /// </summary>
    public DateTime? UltimoPrestamo { get; internal set; }

    /// <summary>
    /// Crea un nuevo libro con los datos proporcionados.
    /// El libro se crea en estado Disponible.
    /// </summary>
    /// <param name="titulo">Título del libro.</param>
    /// <param name="isbn">ISBN del libro.</param>
    /// <param name="autor">Autor del libro.</param>
    /// <param name="utcNow">Fecha y hora actual UTC.</param>
    /// <returns>Resultado con el libro creado.</returns>
    public static Resultado<Libro> Crear(
        Titulo titulo,
        Isbn isbn,
        Autor autor,
        DateTime utcNow
    )
    {
        var libro = new Libro(
            titulo,
            isbn,
            autor,
            EstadoDisponibilidad.Disponible,
            utcNow
        );

        libro.RegistrarEventoDominio(new LibroCreadoEventoDominio(libro.Id));

        return Resultado.Exito(libro);
    }

    /// <summary>
    /// Registra que el libro ha sido prestado.
    /// Valida que el libro esté en estado Disponible antes de permitir el préstamo.
    /// </summary>
    /// <param name="utcNow">Fecha y hora actual UTC.</param>
    /// <returns>Resultado indicando éxito o error si el libro no está disponible.</returns>
    public Resultado PrestarLibro(DateTime utcNow)
    {
        if (Estado != EstadoDisponibilidad.Disponible)
        {
            if (Estado == EstadoDisponibilidad.Prestado)
            {
                return Resultado.Fallo(ErroresLibro.YaEnPrestamo);
            }

            return Resultado.Fallo(ErroresLibro.NoDisponible);
        }

        Estado = EstadoDisponibilidad.Prestado;
        UltimoPrestamo = utcNow;

        RegistrarEventoDominio(new LibroPrestamoRegistradoEventoDominio(Id));

        return Resultado.Exito();
    }

    /// <summary>
    /// Registra la devolución del libro.
    /// Valida que el libro esté en estado Prestado antes de permitir la devolución.
    /// </summary>
    /// <param name="utcNow">Fecha y hora actual UTC.</param>
    /// <returns>Resultado indicando éxito o error si el libro no está en préstamo.</returns>
    public Resultado RegistrarDevolucion(DateTime utcNow)
    {
        if (Estado != EstadoDisponibilidad.Prestado)
        {
            return Resultado.Fallo(ErroresLibro.NoDisponible);
        }

        Estado = EstadoDisponibilidad.Disponible;

        RegistrarEventoDominio(new LibroDevolucionRegistradaEventoDominio(Id));

        return Resultado.Exito();
    }
}
