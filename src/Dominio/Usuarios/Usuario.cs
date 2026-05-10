using Dominio.Abstracciones;
using Dominio.Usuarios.Events;

namespace Dominio.Usuarios;

/// <summary>
/// Entidad que representa un usuario o miembro de la biblioteca.
/// Un usuario puede realizar préstamos de libros y es responsable de su devolución.
/// Contiene información de identificación personal (nombre, apellido y correo electrónico).
/// </summary>
public sealed class Usuario : Entidad
{
    private Usuario(Nombre nombre, Apellido apellido, CorreoElectronico correoElectronico)
    {
        Nombre = nombre;
        Apellido = apellido;
        CorreoElectronico = correoElectronico;
    }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public Nombre Nombre { get; private set; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public Apellido Apellido { get; private set; }

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public CorreoElectronico CorreoElectronico { get; private set; }

    /// <summary>
    /// Crea un nuevo usuario con los datos proporcionados.
    /// </summary>
    /// <param name="nombre">Nombre del usuario.</param>
    /// <param name="apellido">Apellido del usuario.</param>
    /// <param name="correoElectronico">Correo electrónico del usuario.</param>
    /// <returns>El usuario creado.</returns>
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
