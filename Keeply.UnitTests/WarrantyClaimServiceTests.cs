using Keeply.Application.DTOs.Claims;
using Keeply.Application.Services;
using Keeply.Domain.Entities;
using Keeply.UnitTests.Fakes;

namespace Keeply.UnitTests;

public class WarrantyClaimServiceTests
{
    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenProductNotOwned()
    {
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var product = new Product { Id = Guid.NewGuid(), UserId = otherUserId, Name = "Phone", Store = "Store", PurchaseDate = DateOnly.FromDateTime(DateTime.UtcNow), WarrantyMonths = 12 };
        var service = new WarrantyClaimService(new FakeWarrantyClaimRepository(), new FakeProductRepository([product]));

        var result = await service.CreateAsync(userId, new CreateClaimRequest(product.Id, "Broken"));

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStatusAsync_ReturnsError_ForInvalidStatus()
    {
        var userId = Guid.NewGuid();
        var claim = new WarrantyClaim { Id = Guid.NewGuid(), UserId = userId, ProductId = Guid.NewGuid(), Description = "Broken", Status = "Open", CreatedAt = DateTime.UtcNow };
        var service = new WarrantyClaimService(new FakeWarrantyClaimRepository([claim]), new FakeProductRepository());

        var (success, error, result) = await service.UpdateStatusAsync(claim.Id, userId, new UpdateClaimStatusRequest("BadStatus"));

        Assert.False(success);
        Assert.Equal("Invalid status.", error);
        Assert.Null(result);
    }
}
