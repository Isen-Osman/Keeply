using System.ComponentModel.DataAnnotations;

namespace Keeply.Application.DTOs.Auth;

public record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password);
