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

    public async Task<List<Product>> GetAllAsync(Guid userId)
    {
        return await _dbContext.Products.AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
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
