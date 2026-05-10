namespace Dominio.Abstracciones;

/// <summary>
/// Representa un error de dominio con un código único y un mensaje legible.
/// Los errores del dominio son el resultado esperado de operaciones que pueden fallar
/// de acuerdo con las reglas de negocio.
/// </summary>
/// <param name="Codigo">Código único del error (ej: "Usuario.NoEncontrado").</param>
/// <param name="Nombre">Mensaje legible describiendo el error.</param>
public record Error(string Codigo, string Nombre)
{
    /// <summary>
    /// Error especial que indica ausencia de error.
    /// </summary>
    public static Error Ninguno = new(string.Empty, string.Empty);

    /// <summary>
    /// Error estándar cuando se proporciona un valor nulo inesperadamente.
    /// </summary>
    public static Error ValorNulo = new("Error.ValorNulo", "Se proporcionó un valor nulo");
}
