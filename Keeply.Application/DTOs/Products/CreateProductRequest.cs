namespace Keeply.Application.DTOs.Products;

public record CreateProductRequest(
    string Name,
    string Store,
    decimal Price,
    DateOnly PurchaseDate,
    int WarrantyMonths);
