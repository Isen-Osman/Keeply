namespace Keeply.Application.DTOs.Claims;

public record ClaimResponse(
    Guid Id,
    Guid ProductId,
    string Description,
    string Status,
    DateTime CreatedAt);
