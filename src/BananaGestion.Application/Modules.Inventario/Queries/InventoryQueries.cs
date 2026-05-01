using BananaGestion.Application.Modules.Inventario.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Inventario.Queries;

public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

public record GetLowStockProductsQuery : IRequest<IEnumerable<ProductDto>>;

public record GetInventoryMovementsQuery(Guid? ProductId = null, DateTime? Start = null, DateTime? End = null) : IRequest<IEnumerable<InventoryMovementDto>>;
