namespace BananaGestion.Domain.Entities;

public class OrderBoxDetail
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HarvestOrderId { get; set; }
    public Guid BoxTypeId { get; set; }
    public int CantidadPlanificada { get; set; }
    public int CantidadEmpacada { get; set; }
    public string? Notas { get; set; }

    public virtual HarvestOrder HarvestOrder { get; set; } = null!;
    public virtual BoxType BoxType { get; set; } = null!;
}
