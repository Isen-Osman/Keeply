namespace Keeply.Application.DTOs.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    string Store,
    decimal Price,
    DateOnly PurchaseDate,
    int WarrantyMonths,
    DateOnly WarrantyExpirationDate);
