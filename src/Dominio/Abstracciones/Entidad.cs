namespace Dominio.Abstracciones;

/// <summary>
/// Clase base para todas las entidades del dominio.
/// Una entidad es un objeto con identidad única (GUID) que persiste en el tiempo.
/// Proporciona capacidad de registrar y gestionar eventos de dominio.
/// </summary>
public abstract class Entidad
{
    private readonly List<IEventoDominio> eventosDominio = [];

    /// <summary>
    /// Identificador único de la entidad.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Obtiene todos los eventos de dominio registrados por la entidad.
    /// </summary>
    /// <returns>Lista de solo lectura de eventos de dominio.</returns>
    public IReadOnlyList<IEventoDominio> ObtenerEventosDominio()
    {
        return eventosDominio.ToList();
    }

    /// <summary>
    /// Limpia todos los eventos de dominio registrados.
    /// Se utiliza después de que los eventos han sido publicados.
    /// </summary>
    public void LimpiarEventosDominio()
    {
        eventosDominio.Clear();
    }

    /// <summary>
    /// Registra un evento de dominio dentro de la entidad.
    /// </summary>
    /// <param name="eventoDominio">El evento a registrar.</param>
    protected void RegistrarEventoDominio(IEventoDominio eventoDominio)
    {
        eventosDominio.Add(eventoDominio);
    }
}
