using Keeply.Application.DTOs.Auth;
using Keeply.Application.Services;
using Keeply.Domain.Entities;
using Keeply.UnitTests.Fakes;

namespace Keeply.UnitTests;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ReturnsTokens_WhenNewEmail()
    {
        var service = CreateService();

        var result = await service.RegisterAsync(new RegisterRequest("a@b.com", "password"));

        Assert.NotNull(result);
        Assert.StartsWith("token-", result!.Token);
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
    }

    [Fact]
    public async Task RegisterAsync_ReturnsNull_WhenEmailExists()
    {
        var repo = new FakeUserRepository([new User { Email = "a@b.com" }]);
        var service = CreateService(repo);

        var result = await service.RegisterAsync(new RegisterRequest("a@b.com", "password"));

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsNull_ForWrongPassword()
    {
        var repo = new FakeUserRepository([new User { Id = Guid.NewGuid(), Email = "a@b.com", PasswordHash = "HASH:correct" }]);
        var service = CreateService(repo);

        var result = await service.LoginAsync(new LoginRequest("a@b.com", "wrong"));

        Assert.Null(result);
    }

    [Fact]
    public async Task RefreshAsync_ReturnsNull_ForExpiredToken()
    {
        var repo = new FakeUserRepository([new User { Id = Guid.NewGuid(), Email = "a@b.com", RefreshToken = "rt", RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-1) }]);
        var service = CreateService(repo);

        var result = await service.RefreshAsync(new RefreshRequest("rt"));

        Assert.Null(result);
    }

    [Fact]
    public async Task LogoutAsync_ClearsRefreshToken()
    {
        var user = new User { Id = Guid.NewGuid(), Email = "a@b.com", RefreshToken = "rt", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1) };
        var repo = new FakeUserRepository([user]);
        var service = CreateService(repo);

        var ok = await service.LogoutAsync("rt");

        Assert.True(ok);
        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
    }

    private static AuthService CreateService(FakeUserRepository? repo = null) =>
        new(repo ?? new FakeUserRepository(), new FakePasswordHasher(), new FakeJwtTokenGenerator());
}
