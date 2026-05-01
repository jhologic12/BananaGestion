using BananaGestion.Application.Modules.Inventario.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Inventario.Commands;

public record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductDto>;

public record UpdateProductCommand(Guid Id, UpdateProductRequest Request) : IRequest<ProductDto>;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;

public record CreateInventoryMovementCommand(CreateInventoryMovementRequest Request, Guid UserId) : IRequest<InventoryMovementDto>;
