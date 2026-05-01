namespace BananaGestion.Domain.Entities;

public class HarvestCalendar
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Semana { get; set; }
    public int Ano { get; set; }
    public string ColorCinta { get; set; } = string.Empty;
    public string? ColorNombre { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool Activo { get; set; } = true;

    public virtual ICollection<HarvestRecord> HarvestRecords { get; set; } = new List<HarvestRecord>();
}
