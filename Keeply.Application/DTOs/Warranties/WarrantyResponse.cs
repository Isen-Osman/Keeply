namespace Keeply.Application.DTOs.Warranties;

public record WarrantyResponse(
    Guid ProductId,
    string ProductName,
    string Store,
    DateOnly PurchaseDate,
    int WarrantyMonths,
    DateOnly WarrantyExpirationDate);
