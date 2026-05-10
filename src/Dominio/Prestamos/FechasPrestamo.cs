namespace Dominio.Prestamos;

/// <summary>
/// Value Object que encapsula las fechas de un préstamo.
/// Contiene la fecha de inicio del préstamo y opcionalmente la fecha de devolución.
/// </summary>
/// <param name="FechaPrestamoUtc">Fecha y hora en UTC del inicio del préstamo.</param>
/// <param name="FechaDevolucionUtc">Fecha y hora en UTC de la devolución, o null si aún está prestado.</param>
public record FechasPrestamo(DateTime FechaPrestamoUtc, DateTime? FechaDevolucionUtc);
