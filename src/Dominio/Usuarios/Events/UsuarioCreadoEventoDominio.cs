using Dominio.Abstracciones;

namespace Dominio.Usuarios.Events;

public sealed record UsuarioCreadoEventoDominio(Guid UsuarioId) : IEventoDominio;
