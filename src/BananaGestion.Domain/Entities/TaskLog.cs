namespace BananaGestion.Domain.Entities;

public class TaskLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaskAssignmentId { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public string? FotoUrl { get; set; }
    public string? FirmaUrl { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? Observaciones { get; set; }
    public int? CantidadInsumoUtilizado { get; set; }

    public virtual TaskAssignment TaskAssignment { get; set; } = null!;
}
