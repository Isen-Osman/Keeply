using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.UnitTests.Fakes;

public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public FakeProductRepository(IEnumerable<Product>? products = null)
    {
        if (products is not null)
            _products.AddRange(products);
    }

    public Task<List<Product>> GetAllAsync(Guid userId)
    {
        return Task.FromResult(_products.Where(p => p.UserId == userId).ToList());
    }

    public Task<Product?> GetByIdAsync(Guid id, Guid userId)
    {
        return Task.FromResult(_products.FirstOrDefault(p => p.Id == id && p.UserId == userId));
    }

    public Task AddAsync(Product product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product product)
    {
        _products.Remove(product);
        return Task.CompletedTask;
    }
}
