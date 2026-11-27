using System.ComponentModel.DataAnnotations;

namespace Keeply.Application.DTOs.Products;

public record UpdateProductRequest(
    [Required, MaxLength(200)] string Name,
    [Required, MaxLength(200)] string Store,
    [Range(0, double.MaxValue)] decimal Price,
    DateOnly PurchaseDate,
    [Range(0, 600)] int WarrantyMonths);
