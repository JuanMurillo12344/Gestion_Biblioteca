namespace Dominio.Prestamos;

public interface IRepositorioPrestamos
{

Task<Prestamo?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

Task<List<Prestamo>> ObtenerPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);

Task<List<Prestamo>> ObtenerPorLibroIdAsync(Guid libroId, CancellationToken cancellationToken = default);
}
