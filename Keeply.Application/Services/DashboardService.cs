using Keeply.Application.DTOs.Dashboard;
using Keeply.Application.Interfaces;

namespace Keeply.Application.Services;

public class DashboardService
{
    private readonly IProductRepository _productRepository;
    private readonly IWarrantyClaimRepository _claimRepository;
    private readonly IDocumentRepository _documentRepository;

    public DashboardService(
        IProductRepository productRepository,
        IWarrantyClaimRepository claimRepository,
        IDocumentRepository documentRepository)
    {
        _productRepository = productRepository;
        _claimRepository = claimRepository;
        _documentRepository = documentRepository;
    }

    public async Task<DashboardSummaryResponse> GetSummaryAsync(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var expiringLimit = today.AddDays(30);

        var products = await _productRepository.GetAllAsync(userId);
        var claims = await _claimRepository.GetAllAsync(userId);
        var totalDocuments = await _documentRepository.CountByUserIdAsync(userId);

        var expired = products.Count(p => p.WarrantyExpirationDate < today);
        var expiring = products.Count(p => p.WarrantyExpirationDate >= today && p.WarrantyExpirationDate <= expiringLimit);
        var active = products.Count - expired;
        var openClaims = claims.Count(c => c.Status == "Open");

        return new DashboardSummaryResponse(
            products.Count,
            active,
            expiring,
            expired,
            openClaims,
            totalDocuments);
    }
}
