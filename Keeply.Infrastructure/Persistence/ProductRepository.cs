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
}
