namespace Dominio.Usuarios;

/// <summary>
/// Value Object que representa el correo electrónico de un usuario.
/// Encapsula la validación y lógica relacionada con el correo electrónico.
/// </summary>
/// <param name="Valor">El correo electrónico del usuario.</param>
public record CorreoElectronico(string Valor);
