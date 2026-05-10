using System.Diagnostics.CodeAnalysis;

namespace Dominio.Abstracciones;

/// <summary>
/// Implementación del patrón Result (Railway-Oriented Programming).
/// Utilizado para comunicar tanto éxitos como fallos en las operaciones del dominio.
/// No genera excepciones; en su lugar, devuelve un objeto que indica el resultado.
/// </summary>
public class Resultado
{
    protected internal Resultado(bool esExitoso, Error error)
    {
        if (esExitoso && error != Error.Ninguno)
        {
            throw new InvalidOperationException();
        }

        if (!esExitoso && error == Error.Ninguno)
        {
            throw new InvalidOperationException();
        }

        EsExitoso = esExitoso;
        Error = error;
    }

    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool EsExitoso { get; }

    /// <summary>
    /// Indica si la operación falló.
    /// </summary>
    public bool EsFallo => !EsExitoso;

    /// <summary>
    /// El error asociado a esta operación, si falló.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Crea un resultado exitoso sin valor.
    /// </summary>
    public static Resultado Exito() => new(true, Error.Ninguno);

    /// <summary>
    /// Crea un resultado fallido con un error específico.
    /// </summary>
    public static Resultado Fallo(Error error) => new(false, error);

    /// <summary>
    /// Crea un resultado exitoso con un valor genérico.
    /// </summary>
    public static Resultado<TValor> Exito<TValor>(TValor valor) => new(valor, true, Error.Ninguno);

    /// <summary>
    /// Crea un resultado fallido con un error específico y tipo genérico.
    /// </summary>
    public static Resultado<TValor> Fallo<TValor>(Error error) => new(default, false, error);

    /// <summary>
    /// Crea un resultado a partir de un valor, devolviendo éxito si es no-nulo, fallo si es nulo.
    /// </summary>
    public static Resultado<TValor> Crear<TValor>(TValor? valor) =>
        valor is not null ? Exito(valor) : Fallo<TValor>(Error.ValorNulo);
}

/// <summary>
/// Versión genérica del patrón Result que contiene un valor específico.
/// </summary>
/// <typeparam name="TValor">El tipo del valor contenido en caso de éxito.</typeparam>
public class Resultado<TValor> : Resultado
{
    private readonly TValor? _valor;

    protected internal Resultado(TValor? valor, bool esExitoso, Error error)
        : base(esExitoso, error)
    {
        _valor = valor;
    }

    /// <summary>
    /// Obtiene el valor del resultado exitoso.
    /// Lanza InvalidOperationException si el resultado es un fallo.
    /// </summary>
    [NotNull]
    public TValor Valor =>
        EsExitoso
            ? _valor!
            : throw new InvalidOperationException(
                "No se puede acceder al valor de un resultado fallido."
            );

    public static implicit operator Resultado<TValor>(TValor? valor) => Crear(valor);
}
