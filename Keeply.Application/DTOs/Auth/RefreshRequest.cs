using System.ComponentModel.DataAnnotations;

namespace Keeply.Application.DTOs.Auth;

public record RefreshRequest([Required] string RefreshToken);
