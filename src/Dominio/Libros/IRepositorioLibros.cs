namespace Dominio.Libros;

public interface IRepositorioLibros
{

Task<Libro?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

Task<Libro?> ObtenerPorIsbnAsync(Isbn isbn, CancellationToken cancellationToken = default);
}
