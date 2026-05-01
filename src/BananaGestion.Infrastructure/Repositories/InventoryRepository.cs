using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Domain.Entities;
using BananaGestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly BananaDbContext _context;

    public InventoryRepository(BananaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products.Where(p => p.Activo).ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product?> GetProductByCodeAsync(string code)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Codigo == code);
    }

    public async Task<IEnumerable<InventoryMovement>> GetMovementsAsync(Guid? productId = null, DateTime? start = null, DateTime? end = null)
    {
        var query = _context.InventoryMovements
            .Include(m => m.Product)
            .Include(m => m.User)
            .Include(m => m.Lote)
            .AsQueryable();

        if (productId.HasValue)
            query = query.Where(m => m.ProductId == productId.Value);
        if (start.HasValue)
            query = query.Where(m => m.Fecha >= start.Value);
        if (end.HasValue)
            query = query.Where(m => m.Fecha <= end.Value);

        return await query.OrderByDescending(m => m.Fecha).ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<InventoryMovement> CreateMovementAsync(InventoryMovement movement)
    {
        var product = await _context.Products.FindAsync(movement.ProductId);
        if (product == null)
            throw new InvalidOperationException("Product not found");

        movement.StockAnterior = product.StockActual;
        
        if (movement.Tipo == "Entrada")
        {
            movement.StockNuevo = product.StockActual + movement.Cantidad;
            product.StockActual = movement.StockNuevo;
        }
        else
        {
            movement.StockNuevo = product.StockActual - movement.Cantidad;
            product.StockActual = movement.StockNuevo;
        }

        await _context.InventoryMovements.AddAsync(movement);
        await _context.SaveChangesAsync();
        
        return movement;
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
    {
        return await _context.Products
            .Where(p => p.Activo && p.StockActual <= p.StockMinimo)
            .ToListAsync();
    }

    public async Task<decimal> GetCurrentStockAsync(Guid productId)
    {
        var product = await _context.Products.FindAsync(productId);
        return product?.StockActual ?? 0;
    }
}
