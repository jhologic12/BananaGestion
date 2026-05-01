using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Application.Modules.Inventario.Commands;
using BananaGestion.Application.Modules.Inventario.DTOs;
using BananaGestion.Application.Modules.Inventario.Queries;
using BananaGestion.Domain.Entities;
using MediatR;

namespace BananaGestion.Application.Modules.Inventario.Handlers;

public class InventoryHandlers :
    IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>,
    IRequestHandler<GetProductByIdQuery, ProductDto>,
    IRequestHandler<GetLowStockProductsQuery, IEnumerable<ProductDto>>,
    IRequestHandler<GetInventoryMovementsQuery, IEnumerable<InventoryMovementDto>>,
    IRequestHandler<CreateProductCommand, ProductDto>,
    IRequestHandler<UpdateProductCommand, ProductDto>,
    IRequestHandler<DeleteProductCommand, bool>,
    IRequestHandler<CreateInventoryMovementCommand, InventoryMovementDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IRepository<Product> _productRepository;

    public InventoryHandlers(IInventoryRepository inventoryRepository, IRepository<Product> productRepository)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _inventoryRepository.GetProductsAsync();
        return products.Select(MapProductToDto);
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _inventoryRepository.GetProductByIdAsync(request.Id);
        if (product == null)
            throw new InvalidOperationException("Producto no encontrado");
        return MapProductToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _inventoryRepository.GetLowStockProductsAsync();
        return products.Select(MapProductToDto);
    }

    public async Task<IEnumerable<InventoryMovementDto>> Handle(GetInventoryMovementsQuery request, CancellationToken cancellationToken)
    {
        var movements = await _inventoryRepository.GetMovementsAsync(request.ProductId, request.Start, request.End);
        return movements.Select(MapMovementToDto);
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _inventoryRepository.GetProductByCodeAsync(request.Request.Codigo);
        if (existing != null)
            throw new InvalidOperationException("Ya existe un producto con este código");

        var product = new Product
        {
            Codigo = request.Request.Codigo,
            Nombre = request.Request.Nombre,
            Descripcion = request.Request.Descripcion,
            Unidad = request.Request.Unidad,
            StockMinimo = request.Request.StockMinimo,
            StockActual = 0
        };

        await _productRepository.AddAsync(product);
        return MapProductToDto(product);
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _inventoryRepository.GetProductByIdAsync(request.Id);
        if (product == null)
            throw new InvalidOperationException("Producto no encontrado");

        if (!string.IsNullOrEmpty(request.Request.Nombre))
            product.Nombre = request.Request.Nombre;
        if (request.Request.Descripcion != null)
            product.Descripcion = request.Request.Descripcion;
        if (!string.IsNullOrEmpty(request.Request.Unidad))
            product.Unidad = request.Request.Unidad;
        if (request.Request.StockMinimo.HasValue)
            product.StockMinimo = request.Request.StockMinimo.Value;
        if (request.Request.Activo.HasValue)
            product.Activo = request.Request.Activo.Value;

        await _inventoryRepository.UpdateProductAsync(product);
        return MapProductToDto(product);
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _inventoryRepository.GetProductByIdAsync(request.Id);
        if (product != null)
        {
            product.Activo = false;
            await _inventoryRepository.UpdateProductAsync(product);
        }
        return true;
    }

    public async Task<InventoryMovementDto> Handle(CreateInventoryMovementCommand request, CancellationToken cancellationToken)
    {
        var product = await _inventoryRepository.GetProductByIdAsync(request.Request.ProductId);
        if (product == null)
            throw new InvalidOperationException("Producto no encontrado");

        var movement = new InventoryMovement
        {
            ProductId = request.Request.ProductId,
            UserId = request.UserId,
            LoteId = request.Request.LoteId,
            Tipo = request.Request.Tipo,
            Cantidad = request.Request.Cantidad,
            Referencia = request.Request.Referencia,
            Notas = request.Request.Notas
        };

        var created = await _inventoryRepository.CreateMovementAsync(movement);
        var fullMovement = (await _inventoryRepository.GetMovementsAsync(request.Request.ProductId))
            .FirstOrDefault(m => m.Id == created.Id);

        return MapMovementToDto(fullMovement!);
    }

    private ProductDto MapProductToDto(Product p) => new(
        p.Id, p.Codigo, p.Nombre, p.Descripcion, p.Unidad,
        p.StockMinimo, p.StockActual, p.Activo, p.StockActual <= p.StockMinimo
    );

    private InventoryMovementDto MapMovementToDto(InventoryMovement m) => new(
        m.Id, m.ProductId, m.Product.Nombre, m.LoteId, m.Lote?.Nombre,
        m.UserId, m.User != null ? $"{m.User.Nombre} {m.User.Apellido}" : null,
        m.Tipo, m.Cantidad, m.StockAnterior, m.StockNuevo,
        m.Referencia, m.Notas, m.Fecha
    );
}
