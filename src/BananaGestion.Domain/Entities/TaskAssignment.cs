using BananaGestion.Domain.Enums;

namespace BananaGestion.Domain.Entities;

public class TaskAssignment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaskConfigId { get; set; }
    public Guid UserId { get; set; }
    public Guid LoteId { get; set; }
    public DateTime FechaProgramada { get; set; }
    public DateTime? FechaCompletada { get; set; }
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Programada;
    public string? Notas { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public virtual TaskConfig TaskConfig { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Lote Lote { get; set; } = null!;
    public virtual TaskLog? TaskLog { get; set; }
}
