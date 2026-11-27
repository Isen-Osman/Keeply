using Keeply.Application.Services;
using Keeply.Domain.Entities;
using Keeply.UnitTests.Fakes;

namespace Keeply.UnitTests;

public class DocumentServiceTests
{
    [Fact]
    public async Task UploadGetDelete_WorksForOwnedProduct()
    {
        var userId = Guid.NewGuid();
        var product = new Product { Id = Guid.NewGuid(), UserId = userId, Name = "P", Store = "S", PurchaseDate = DateOnly.FromDateTime(DateTime.UtcNow), WarrantyMonths = 12 };
        var service = new DocumentService(new FakeDocumentRepository(), new FakeProductRepository([product]));
        using var stream = new MemoryStream([1, 2, 3]);

        var uploaded = await service.UploadAsync(userId, product.Id, "receipt.txt", stream, stream.Length);
        Assert.NotNull(uploaded);

        var (doc, path) = await service.GetByIdAsync(uploaded!.Id, userId);
        Assert.NotNull(doc);
        Assert.True(File.Exists(path));

        var deleted = await service.DeleteAsync(uploaded.Id, userId);
        Assert.True(deleted);
        Assert.False(File.Exists(path!));
    }
}
