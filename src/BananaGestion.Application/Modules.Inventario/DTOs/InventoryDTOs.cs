namespace BananaGestion.Application.Modules.Inventario.DTOs;

public record ProductDto(
    Guid Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    string Unidad,
    decimal StockMinimo,
    decimal StockActual,
    bool Activo,
    bool IsLowStock
);

public record InventoryMovementDto(
    Guid Id,
    Guid ProductId,
    string ProductNombre,
    Guid? LoteId,
    string? LoteNombre,
    Guid? UserId,
    string? UserNombre,
    string Tipo,
    decimal Cantidad,
    decimal StockAnterior,
    decimal StockNuevo,
    string? Referencia,
    string? Notas,
    DateTime Fecha
);

public record CreateProductRequest(
    string Codigo,
    string Nombre,
    string? Descripcion,
    string Unidad,
    decimal StockMinimo
);

public record UpdateProductRequest(
    string? Nombre,
    string? Descripcion,
    string? Unidad,
    decimal? StockMinimo,
    bool? Activo
);

public record CreateInventoryMovementRequest(
    Guid ProductId,
    string Tipo,
    decimal Cantidad,
    Guid? LoteId,
    string? Referencia,
    string? Notas
);
