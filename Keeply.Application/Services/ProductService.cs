using Keeply.Application.DTOs.Products;
using Keeply.Application.Interfaces;

namespace Keeply.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(p => new ProductResponse(
            p.Id,
            p.Name,
            p.Store,
            p.Price,
            p.PurchaseDate,
            p.WarrantyMonths,
            p.WarrantyExpirationDate)).ToList();
    }
}
