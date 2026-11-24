using Keeply.Domain.Entities;

namespace Keeply.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
}
