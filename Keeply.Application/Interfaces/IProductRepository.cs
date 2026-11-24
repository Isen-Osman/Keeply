using Keeply.Domain.Entities;

namespace Keeply.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(Guid userId);
    Task<Product?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
