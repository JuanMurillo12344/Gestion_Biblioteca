using Dominio.Abstracciones;

namespace Dominio.Libros.Events;

public sealed record LibroDevolucionRegistradaEventoDominio(Guid LibroId) : IEventoDominio;
