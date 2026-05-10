namespace Dominio.Prestamos;

/// <summary>
/// Enumeración que representa los posibles estados de un préstamo.
/// </summary>
public enum EstadoPrestamo
{
    /// <summary>
    /// El préstamo está activo (el usuario tiene el libro).
    /// </summary>
    Activo = 0,

    /// <summary>
    /// El préstamo ha sido completado (el libro ha sido devuelto).
    /// </summary>
    Devuelto = 1,

    /// <summary>
    /// El préstamo ha expirado (la fecha de devolución ha pasado).
    /// </summary>
    Vencido = 2
}
