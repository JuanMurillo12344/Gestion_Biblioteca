using Dominio.Abstracciones;

namespace Dominio.Libros.Events;

public sealed record LibroCreadoEventoDominio(Guid LibroId) : IEventoDominio;
