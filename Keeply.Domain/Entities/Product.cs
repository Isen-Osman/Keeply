namespace Keeply.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Store { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public int WarrantyMonths { get; set; }

    public DateOnly WarrantyExpirationDate => PurchaseDate.AddMonths(WarrantyMonths);
}
