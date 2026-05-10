using Dominio.Abstracciones;

namespace Dominio.Prestamos.Events;

public sealed record DevolucionRegistradaEventoDominio(Guid PrestamoId, Guid LibroId, Guid UsuarioId) : IEventoDominio;
