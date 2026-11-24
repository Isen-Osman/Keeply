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

    public async Task<List<ProductResponse>> GetAllAsync(Guid userId)
    {
        var products = await _productRepository.GetAllAsync(userId);
        return products.Select(MapToResponse).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(id, userId);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, Guid userId)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name,
            Store = request.Store,
            Price = request.Price,
            PurchaseDate = request.PurchaseDate,
            WarrantyMonths = request.WarrantyMonths
        };

        await _productRepository.AddAsync(product);
        return MapToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(id, userId);
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

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(id, userId);
        if (product is null)
            return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.UserId,
            product.Name,
            product.Store,
            product.Price,
            product.PurchaseDate,
            product.WarrantyMonths,
            product.WarrantyExpirationDate);
    }
}
