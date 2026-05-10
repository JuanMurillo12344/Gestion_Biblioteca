namespace Dominio.Usuarios;

/// <summary>
/// Contrato del repositorio para la persistencia de usuarios.
/// Define las operaciones de acceso a datos para la entidad Usuario en el agregado de Usuarios.
/// </summary>
public interface IRepositorioUsuarios
{
    /// <summary>
    /// Obtiene un usuario por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>El usuario si existe, null en caso contrario.</returns>
    Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega un nuevo usuario al repositorio.
    /// </summary>
    /// <param name="usuario">El usuario a agregar.</param>
    void Agregar(Usuario usuario);
}
