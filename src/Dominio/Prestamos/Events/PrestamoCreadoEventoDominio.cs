using Dominio.Abstracciones;

namespace Dominio.Prestamos.Events;

public sealed record PrestamoCreadoEventoDominio(Guid PrestamoId, Guid LibroId, Guid UsuarioId) : IEventoDominio;
