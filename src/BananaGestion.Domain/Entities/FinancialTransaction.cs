namespace BananaGestion.Domain.Entities;

public class FinancialTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Tipo { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string? Descripcion { get; set; }
    public Guid? ReferenciaId { get; set; }
    public string? ReferenciaTipo { get; set; }
    public string? ComprobanteUrl { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
