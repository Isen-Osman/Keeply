using System.ComponentModel.DataAnnotations;

namespace Keeply.Application.DTOs.Claims;

public record UpdateClaimStatusRequest([Required] string Status);
