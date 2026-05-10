namespace Dominio.Libros;

/// <summary>
/// Enumeración que representa los posibles estados de disponibilidad de un libro.
/// </summary>
public enum EstadoDisponibilidad
{
    /// <summary>
    /// El libro está disponible para ser prestado.
    /// </summary>
    Disponible = 0,

    /// <summary>
    /// El libro está siendo prestado actualmente.
    /// </summary>
    Prestado = 1,

    /// <summary>
    /// El libro no está disponible (retirado, perdido, dañado, etc).
    /// </summary>
    NoDisponible = 2
}
