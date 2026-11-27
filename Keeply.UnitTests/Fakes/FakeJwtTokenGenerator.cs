using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.UnitTests.Fakes;

public class FakeJwtTokenGenerator : IJwtTokenGenerator
{
    public string Generate(User user) => $"token-{user.Id}";
}
