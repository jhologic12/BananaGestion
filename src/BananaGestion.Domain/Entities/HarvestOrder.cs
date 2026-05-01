namespace BananaGestion.Domain.Entities;

public class HarvestOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NumeroOrden { get; set; } = string.Empty;
    public Guid? HarvestCalendarId { get; set; }
    public DateTime FechaEmbarque { get; set; }
    public string? PdfUrl { get; set; }
    public string? Cliente { get; set; }
    public string? Notas { get; set; }
    public bool Procesada { get; set; } = false;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public virtual HarvestCalendar? HarvestCalendar { get; set; }
    public virtual ICollection<OrderBoxDetail> BoxDetails { get; set; } = new List<OrderBoxDetail>();
}
