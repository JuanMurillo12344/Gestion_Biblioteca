namespace Dominio.Libros;

/// <summary>
/// Value Object que representa el autor de un libro.
/// Encapsula la validación y lógica relacionada con el nombre del autor.
/// </summary>
/// <param name="Valor">El nombre del autor del libro.</param>
public record Autor(string Valor);
