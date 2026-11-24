using Keeply.Application.DTOs.Products;
using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

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
        return products.Select(MapToResponse).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Store = request.Store,
            Price = request.Price,
            PurchaseDate = request.PurchaseDate,
            WarrantyMonths = request.WarrantyMonths
        };

        await _productRepository.AddAsync(product);
        return MapToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return null;

        product.Name = request.Name;
        product.Store = request.Store;
        product.Price = request.Price;
        product.PurchaseDate = request.PurchaseDate;
        product.WarrantyMonths = request.WarrantyMonths;

        await _productRepository.UpdateAsync(product);
        return MapToResponse(product);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Store,
            product.Price,
            product.PurchaseDate,
            product.WarrantyMonths,
            product.WarrantyExpirationDate);
    }
}
