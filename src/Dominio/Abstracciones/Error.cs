namespace Dominio.Abstracciones;

public record Error(string Codigo, string Nombre)
{

public static Error Ninguno = new(string.Empty, string.Empty);

public static Error ValorNulo = new("Error.ValorNulo", "Se proporcionó un valor nulo");
}
