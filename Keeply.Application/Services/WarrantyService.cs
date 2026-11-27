using Keeply.Application.DTOs.Warranties;
using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.Application.Services;

public class WarrantyService
{
    private readonly IProductRepository _productRepository;

    public WarrantyService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<WarrantyResponse>> GetAllAsync(Guid userId)
    {
        var products = await _productRepository.GetAllAsync(userId);
        return products.Select(MapToResponse).ToList();
    }

    public async Task<List<WarrantyResponse>> GetExpiringAsync(Guid userId, int days = 30)
    {
        if (days <= 0) days = 30;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var limit = today.AddDays(days);
        var products = await _productRepository.GetAllAsync(userId);

        return products
            .Where(p => p.WarrantyExpirationDate >= today && p.WarrantyExpirationDate <= limit)
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<List<WarrantyResponse>> GetExpiredAsync(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var products = await _productRepository.GetAllAsync(userId);

        return products
            .Where(p => p.WarrantyExpirationDate < today)
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<WarrantyResponse?> GetByIdAsync(Guid productId, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(productId, userId);
        return product is null ? null : MapToResponse(product);
    }

    private static WarrantyResponse MapToResponse(Product product)
    {
        return new WarrantyResponse(
            product.Id,
            product.Name,
            product.Store,
            product.PurchaseDate,
            product.WarrantyMonths,
            product.WarrantyExpirationDate);
    }
}
