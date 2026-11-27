using System.ComponentModel.DataAnnotations;

namespace Keeply.Application.DTOs.Claims;

public record CreateClaimRequest(
    [Required] Guid ProductId,
    [Required, MaxLength(1000)] string Description);
