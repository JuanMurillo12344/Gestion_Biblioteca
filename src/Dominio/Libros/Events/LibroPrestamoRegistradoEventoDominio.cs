using Dominio.Abstracciones;

namespace Dominio.Libros.Events;

public sealed record LibroPrestamoRegistradoEventoDominio(Guid LibroId) : IEventoDominio;
