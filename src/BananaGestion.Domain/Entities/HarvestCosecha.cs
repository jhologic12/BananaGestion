using BananaGestion.Domain.Enums;

namespace BananaGestion.Domain.Entities;

public class HarvestCosecha
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? HarvestCalendarId { get; set; }
    public Guid LoteId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Fecha { get; set; }
    
    public int SemanaEncinte { get; set; }
    public int AnoEncinte { get; set; }
    public int SemanaCosecha { get; set; }
    public int AnoCosecha { get; set; }
    
    public HarvestState Estado { get; set; }
    public int CantidadRacimos { get; set; }
    
    public string ColorCinta { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public virtual HarvestCalendar? HarvestCalendar { get; set; }
    public virtual Lote Lote { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
