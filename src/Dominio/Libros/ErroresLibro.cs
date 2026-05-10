using Dominio.Abstracciones;

namespace Dominio.Libros;

public static class ErroresLibro
{

public static readonly Error NoEncontrado = new(
        "Libro.NoEncontrado",
        "No se encontró el libro con el identificador especificado"
    );

public static readonly Error NoDisponible = new(
        "Libro.NoDisponible",
        "El libro no está disponible para préstamo"
    );

public static readonly Error YaEnPrestamo = new(
        "Libro.YaEnPrestamo",
        "El libro ya está en préstamo"
    );
}
