using Keeply.Application.DTOs.Products;
using Keeply.Application.Services;
using Keeply.Domain.Entities;
using Keeply.UnitTests.Fakes;

namespace Keeply.UnitTests;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAndGetAll_AreScopedToUser()
    {
        var userId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var repo = new FakeProductRepository([new Product { Id = Guid.NewGuid(), UserId = otherId, Name = "Other", Store = "S", PurchaseDate = DateOnly.FromDateTime(DateTime.UtcNow), WarrantyMonths = 1 }]);
        var service = new ProductService(repo);

        await service.CreateAsync(new CreateProductRequest("Mine", "Store", 10, new DateOnly(2026, 1, 1), 12), userId);
        var mine = await service.GetAllAsync(userId);

        Assert.Single(mine);
        Assert.Equal("Mine", mine[0].Name);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_ForOtherUsersProduct()
    {
        var owner = Guid.NewGuid();
        var product = new Product { Id = Guid.NewGuid(), UserId = owner, Name = "P", Store = "S", PurchaseDate = DateOnly.FromDateTime(DateTime.UtcNow), WarrantyMonths = 1 };
        var service = new ProductService(new FakeProductRepository([product]));

        var deleted = await service.DeleteAsync(product.Id, Guid.NewGuid());

        Assert.False(deleted);
    }
}
