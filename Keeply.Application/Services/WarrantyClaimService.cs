using Keeply.Application.DTOs.Claims;
using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.Application.Services;

public class WarrantyClaimService
{
    private static readonly HashSet<string> AllowedStatuses = ["Open", "InProgress", "Resolved", "Rejected"];

    private readonly IWarrantyClaimRepository _claimRepository;
    private readonly IProductRepository _productRepository;

    public WarrantyClaimService(IWarrantyClaimRepository claimRepository, IProductRepository productRepository)
    {
        _claimRepository = claimRepository;
        _productRepository = productRepository;
    }

    public async Task<List<ClaimResponse>> GetAllAsync(Guid userId)
    {
        var claims = await _claimRepository.GetAllAsync(userId);
        return claims.Select(MapToResponse).ToList();
    }

    public async Task<ClaimResponse?> GetByIdAsync(Guid id, Guid userId)
    {
        var claim = await _claimRepository.GetByIdAsync(id, userId);
        return claim is null ? null : MapToResponse(claim);
    }

    public async Task<ClaimResponse?> CreateAsync(Guid userId, CreateClaimRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, userId);
        if (product is null)
            return null;

        var claim = new WarrantyClaim
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = request.ProductId,
            Description = request.Description,
            Status = "Open",
            CreatedAt = DateTime.UtcNow
        };

        await _claimRepository.AddAsync(claim);
        return MapToResponse(claim);
    }

    public async Task<(bool Success, string? Error, ClaimResponse? Claim)> UpdateStatusAsync(Guid id, Guid userId, UpdateClaimStatusRequest request)
    {
        if (!AllowedStatuses.Contains(request.Status))
            return (false, "Invalid status.", null);

        var claim = await _claimRepository.GetByIdAsync(id, userId);
        if (claim is null)
            return (false, "Claim not found.", null);

        claim.Status = request.Status;
        await _claimRepository.UpdateAsync(claim);
        return (true, null, MapToResponse(claim));
    }

    private static ClaimResponse MapToResponse(WarrantyClaim claim)
    {
        return new ClaimResponse(claim.Id, claim.ProductId, claim.Description, claim.Status, claim.CreatedAt);
    }
}
