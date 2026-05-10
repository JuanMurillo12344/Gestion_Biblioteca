namespace Dominio.Libros;

/// <summary>
/// Contrato del repositorio para la persistencia de libros.
/// Define las operaciones de acceso a datos para la entidad Libro en el agregado de Libros.
/// </summary>
public interface IRepositorioLibros
{
    /// <summary>
    /// Obtiene un libro por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del libro.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>El libro si existe, null en caso contrario.</returns>
    Task<Libro?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un libro por su ISBN.
    /// </summary>
    /// <param name="isbn">ISBN del libro a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>El libro si existe, null en caso contrario.</returns>
    Task<Libro?> ObtenerPorIsbnAsync(Isbn isbn, CancellationToken cancellationToken = default);
}
