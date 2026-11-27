using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.UnitTests.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public FakeUserRepository(IEnumerable<User>? users = null)
    {
        if (users is not null)
            _users.AddRange(users);
    }

    public Task<User?> GetByEmailAsync(string email) =>
        Task.FromResult(_users.FirstOrDefault(u => u.Email == email));

    public Task<User?> GetByRefreshTokenAsync(string refreshToken) =>
        Task.FromResult(_users.FirstOrDefault(u => u.RefreshToken == refreshToken));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user) => Task.CompletedTask;
}
