using Dominio.Abstracciones;
using Dominio.Libros.Events;

namespace Dominio.Libros;

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

public Titulo Titulo { get; private set; }

public Isbn Isbn { get; private set; }

public Autor Autor { get; private set; }

public EstadoDisponibilidad Estado { get; private set; }

public DateTime CreadoEnUtc { get; private set; }

public DateTime? UltimoPrestamo { get; internal set; }

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
