using Dominio.Abstracciones;

namespace Dominio.Prestamos;

/// <summary>
/// Catálogo de errores del módulo de Préstamos.
/// Centraliza la definición de todos los errores de dominio relacionados con préstamos.
/// </summary>
public static class ErroresPrestamo
{
    /// <summary>
    /// El préstamo solicitado no existe.
    /// </summary>
    public static readonly Error NoEncontrado = new(
        "Prestamo.NoEncontrado",
        "No se encontró el préstamo con el identificador especificado"
    );

    /// <summary>
    /// El préstamo ya ha sido devuelto, no puede realizarse otra devolución.
    /// </summary>
    public static readonly Error YaDevuelto = new(
        "Prestamo.YaDevuelto",
        "El préstamo ya ha sido devuelto"
    );

    /// <summary>
    /// El préstamo no está activo, no puede procesarse la operación solicitada.
    /// </summary>
    public static readonly Error NoActivo = new(
        "Prestamo.NoActivo",
        "El préstamo no está activo"
    );

    /// <summary>
    /// El libro no está disponible para prestar.
    /// </summary>
    public static readonly Error LibroNoDisponible = new(
        "Prestamo.LibroNoDisponible",
        "El libro no está disponible para préstamo"
    );
}
