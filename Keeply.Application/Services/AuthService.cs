using System.Security.Cryptography;
using Keeply.Application.DTOs.Auth;
using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing is not null)
            return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        SetRefreshToken(user);
        await _userRepository.AddAsync(user);
        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        SetRefreshToken(user);
        await _userRepository.UpdateAsync(user);
        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse?> RefreshAsync(RefreshRequest request)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        SetRefreshToken(user);
        await _userRepository.UpdateAsync(user);
        return CreateAuthResponse(user);
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if (user is null)
            return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        return new AuthResponse(_jwtTokenGenerator.Generate(user), user.RefreshToken!);
    }

    private static void SetRefreshToken(User user)
    {
        user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    }
}
