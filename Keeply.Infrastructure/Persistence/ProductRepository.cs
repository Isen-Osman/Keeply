using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Keeply.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly KeeplyDbContext _dbContext;

    public ProductRepository(KeeplyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
}
