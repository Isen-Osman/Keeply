using Keeply.Domain.Entities;

namespace Keeply.UnitTests;

public class ProductTests
{
    [Fact]
    public void WarrantyExpirationDate_IsCalculatedFromPurchaseDateAndMonths()
    {
        var product = new Product
        {
            PurchaseDate = new DateOnly(2026, 1, 10),
            WarrantyMonths = 24
        };

        Assert.Equal(new DateOnly(2028, 1, 10), product.WarrantyExpirationDate);
    }
}
