namespace BananaGestion.Domain.Entities;

public class InventoryMovement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Guid? LoteId { get; set; }
    public Guid? UserId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal StockAnterior { get; set; }
    public decimal StockNuevo { get; set; }
    public string? Referencia { get; set; }
    public string? Notas { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; } = null!;
    public virtual Lote? Lote { get; set; }
    public virtual User? User { get; set; }
}
