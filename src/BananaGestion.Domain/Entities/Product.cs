namespace BananaGestion.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public decimal StockMinimo { get; set; }
    public decimal StockActual { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
    public virtual ICollection<TaskConfig> TaskConfigs { get; set; } = new List<TaskConfig>();
}
