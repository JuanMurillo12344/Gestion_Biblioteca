using Dominio.Abstracciones;
using Dominio.Libros;
using Dominio.Prestamos.Events;

namespace Dominio.Prestamos;

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

public Guid LibroId { get; private set; }

public Guid UsuarioId { get; private set; }

public FechasPrestamo Fechas { get; private set; }

public EstadoPrestamo Estado { get; private set; }

public DateTime CreadoEnUtc { get; private set; }

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
