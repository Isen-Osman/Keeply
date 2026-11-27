using Keeply.Application.Services;
using Keeply.Domain.Entities;
using Keeply.UnitTests.Fakes;

namespace Keeply.UnitTests;

public class DashboardServiceTests
{
    [Fact]
    public async Task GetSummaryAsync_ReturnsCounts()
    {
        var userId = Guid.NewGuid();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var products = new FakeProductRepository([
            new Product { Id = Guid.NewGuid(), UserId = userId, Name = "A", Store = "S", PurchaseDate = today, WarrantyMonths = 12 },
            new Product { Id = Guid.NewGuid(), UserId = userId, Name = "B", Store = "S", PurchaseDate = today.AddMonths(-13), WarrantyMonths = 12 },
        ]);
        var claims = new FakeWarrantyClaimRepository([
            new WarrantyClaim { Id = Guid.NewGuid(), UserId = userId, ProductId = Guid.NewGuid(), Description = "d", Status = "Open", CreatedAt = DateTime.UtcNow },
            new WarrantyClaim { Id = Guid.NewGuid(), UserId = userId, ProductId = Guid.NewGuid(), Description = "d", Status = "Resolved", CreatedAt = DateTime.UtcNow },
        ]);
        var docs = new FakeDocumentRepository([new Document { Id = Guid.NewGuid(), UserId = userId, ProductId = Guid.NewGuid(), FileName = "f", FilePath = "p", UploadedAt = DateTime.UtcNow }]);
        var service = new DashboardService(products, claims, docs);

        var summary = await service.GetSummaryAsync(userId);

        Assert.Equal(2, summary.TotalProducts);
        Assert.Equal(1, summary.ExpiredWarranties);
        Assert.Equal(1, summary.OpenClaims);
        Assert.Equal(1, summary.TotalDocuments);
    }
}
