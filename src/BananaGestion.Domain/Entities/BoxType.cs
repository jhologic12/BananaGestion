namespace BananaGestion.Domain.Entities;

public class BoxType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CapacidadKilos { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public virtual ICollection<HarvestBoxRecord> HarvestBoxRecords { get; set; } = new List<HarvestBoxRecord>();
}
