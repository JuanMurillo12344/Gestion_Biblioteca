namespace Dominio.Libros;

/// <summary>
/// Value Object que representa el ISBN (International Standard Book Number) de un libro.
/// Proporciona un identificador estándar internacionalmente único para cada libro.
/// </summary>
/// <param name="Valor">El valor del ISBN del libro.</param>
public record Isbn(string Valor);
