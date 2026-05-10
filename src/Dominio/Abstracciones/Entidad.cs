namespace Dominio.Abstracciones;

public abstract class Entidad
{
    private readonly List<IEventoDominio> eventosDominio = [];

public Guid Id { get; init; } = Guid.NewGuid();

public IReadOnlyList<IEventoDominio> ObtenerEventosDominio()
    {
        return eventosDominio.ToList();
    }

public void LimpiarEventosDominio()
    {
        eventosDominio.Clear();
    }

protected void RegistrarEventoDominio(IEventoDominio eventoDominio)
    {
        eventosDominio.Add(eventoDominio);
    }
}
