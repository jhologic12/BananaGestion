using BananaGestion.Domain.Enums;

namespace BananaGestion.Domain.Entities;

public class HarvestRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? HarvestCalendarId { get; set; }
    public Guid LoteId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Fecha { get; set; }
    public int CantidadRacimosEmbolsados { get; set; }
    public int? RacimosPerdidos { get; set; }
    public string ColorCinta { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    
    public int SemanaEncinte { get; set; }
    public int AnoEncinte { get; set; }

    public virtual HarvestCalendar? HarvestCalendar { get; set; }
    public virtual Lote Lote { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<HarvestBoxRecord> BoxRecords { get; set; } = new List<HarvestBoxRecord>();
}
