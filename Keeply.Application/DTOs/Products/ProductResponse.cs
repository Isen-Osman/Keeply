namespace Keeply.Application.DTOs.Products;

public record ProductResponse(
    Guid Id,
    Guid UserId,
    string Name,
    string Store,
    decimal Price,
    DateOnly PurchaseDate,
    int WarrantyMonths,
    DateOnly WarrantyExpirationDate);
