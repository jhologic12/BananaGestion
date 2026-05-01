namespace BananaGestion.Domain.Entities;

public class HarvestBoxRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HarvestRecordId { get; set; }
    public Guid BoxTypeId { get; set; }
    public int Cantidad { get; set; }
    public decimal? PesoTotal { get; set; }
    public string? Notas { get; set; }

    public virtual HarvestRecord HarvestRecord { get; set; } = null!;
    public virtual BoxType BoxType { get; set; } = null!;
}
