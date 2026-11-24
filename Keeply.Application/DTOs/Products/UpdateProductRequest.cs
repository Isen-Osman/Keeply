namespace Keeply.Application.DTOs.Products;

public record UpdateProductRequest(
    string Name,
    string Store,
    decimal Price,
    DateOnly PurchaseDate,
    int WarrantyMonths);
