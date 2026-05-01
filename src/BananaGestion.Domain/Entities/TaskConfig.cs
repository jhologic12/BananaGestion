using BananaGestion.Domain.Enums;

namespace BananaGestion.Domain.Entities;

public class TaskConfig
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public LaborType TipoLabor { get; set; }
    public TaskType Tipo { get; set; }
    public int? FrecuenciaDias { get; set; }
    public decimal? InsumoCantidad { get; set; }
    public Guid? InsumoId { get; set; }
    public bool RequiereFoto { get; set; } = true;
    public bool RequiereFirma { get; set; } = true;
    public bool RequiereGps { get; set; } = false;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public virtual Product? Insumo { get; set; }
    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    public virtual ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
}
