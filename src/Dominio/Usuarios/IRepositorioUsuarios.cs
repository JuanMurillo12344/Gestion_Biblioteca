namespace Dominio.Usuarios;

public interface IRepositorioUsuarios
{

Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

void Agregar(Usuario usuario);
}
