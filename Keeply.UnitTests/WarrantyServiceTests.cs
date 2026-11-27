using Keeply.Application.Services;
using Keeply.Domain.Entities;
using Keeply.UnitTests.Fakes;

namespace Keeply.UnitTests;

public class WarrantyServiceTests
{
    [Fact]
    public async Task GetExpiringAndExpired_FilterCorrectly()
    {
        var userId = Guid.NewGuid();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var repo = new FakeProductRepository([
            new Product { Id = Guid.NewGuid(), UserId = userId, Name = "Expiring", Store = "S", PurchaseDate = today.AddMonths(-11).AddDays(-20), WarrantyMonths = 12 },
            new Product { Id = Guid.NewGuid(), UserId = userId, Name = "Expired", Store = "S", PurchaseDate = today.AddMonths(-13), WarrantyMonths = 12 },
            new Product { Id = Guid.NewGuid(), UserId = userId, Name = "Active", Store = "S", PurchaseDate = today, WarrantyMonths = 12 },
        ]);
        var service = new WarrantyService(repo);

        var expiring = await service.GetExpiringAsync(userId);
        var expired = await service.GetExpiredAsync(userId);

        Assert.Single(expiring);
        Assert.Equal("Expiring", expiring[0].ProductName);
        Assert.Single(expired);
        Assert.Equal("Expired", expired[0].ProductName);
    }
}
