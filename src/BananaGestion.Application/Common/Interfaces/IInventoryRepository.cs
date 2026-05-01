using BananaGestion.Domain.Entities;

namespace BananaGestion.Application.Common.Interfaces;

public interface IInventoryRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product?> GetProductByCodeAsync(string code);
    Task<IEnumerable<InventoryMovement>> GetMovementsAsync(Guid? productId = null, DateTime? start = null, DateTime? end = null);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task<InventoryMovement> CreateMovementAsync(InventoryMovement movement);
    Task<IEnumerable<Product>> GetLowStockProductsAsync();
    Task<decimal> GetCurrentStockAsync(Guid productId);
}
