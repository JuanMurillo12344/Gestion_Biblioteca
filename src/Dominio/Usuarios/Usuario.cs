using Dominio.Abstracciones;
using Dominio.Usuarios.Events;

namespace Dominio.Usuarios;

public sealed class Usuario : Entidad
{
    private Usuario(Nombre nombre, Apellido apellido, CorreoElectronico correoElectronico)
    {
        Nombre = nombre;
        Apellido = apellido;
        CorreoElectronico = correoElectronico;
    }

public Nombre Nombre { get; private set; }

public Apellido Apellido { get; private set; }

public CorreoElectronico CorreoElectronico { get; private set; }

public static Usuario Crear(
        Nombre nombre,
        Apellido apellido,
        CorreoElectronico correoElectronico
    )
    {
        var usuario = new Usuario(nombre, apellido, correoElectronico);

usuario.RegistrarEventoDominio(new UsuarioCreadoEventoDominio(usuario.Id));

return usuario;
    }
}
