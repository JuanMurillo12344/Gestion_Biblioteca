using Dominio.Abstracciones;

namespace Dominio.Libros;

/// <summary>
/// Catálogo de errores del módulo de Libros.
/// Centraliza la definición de todos los errores de dominio relacionados con libros.
/// </summary>
public static class ErroresLibro
{
    /// <summary>
    /// El libro solicitado no existe en la biblioteca.
    /// </summary>
    public static readonly Error NoEncontrado = new(
        "Libro.NoEncontrado",
        "No se encontró el libro con el identificador especificado"
    );

    /// <summary>
    /// El libro no está disponible para ser prestado.
    /// </summary>
    public static readonly Error NoDisponible = new(
        "Libro.NoDisponible",
        "El libro no está disponible para préstamo"
    );

    /// <summary>
    /// El libro ya está siendo prestado a otro usuario.
    /// </summary>
    public static readonly Error YaEnPrestamo = new(
        "Libro.YaEnPrestamo",
        "El libro ya está en préstamo"
    );
}
