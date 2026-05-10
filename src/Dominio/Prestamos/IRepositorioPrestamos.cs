namespace Dominio.Prestamos;

/// <summary>
/// Contrato del repositorio para la persistencia de préstamos.
/// Define las operaciones de acceso a datos para la entidad Prestamo en el agregado de Préstamos.
/// </summary>
public interface IRepositorioPrestamos
{
    /// <summary>
    /// Obtiene un préstamo por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del préstamo.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>El préstamo si existe, null en caso contrario.</returns>
    Task<Prestamo?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los préstamos de un usuario específico.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>Lista de préstamos del usuario. Puede estar vacía si el usuario no tiene préstamos.</returns>
    Task<List<Prestamo>> ObtenerPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los préstamos de un libro específico.
    /// </summary>
    /// <param name="libroId">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>Lista de préstamos del libro. Puede estar vacía si el libro no tiene préstamos.</returns>
    Task<List<Prestamo>> ObtenerPorLibroIdAsync(Guid libroId, CancellationToken cancellationToken = default);
}
