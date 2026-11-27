using System.ComponentModel.DataAnnotations;

namespace Keeply.Application.DTOs.Auth;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);
