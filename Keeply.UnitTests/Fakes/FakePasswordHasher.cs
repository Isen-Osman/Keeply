using Keeply.Application.Interfaces;

namespace Keeply.UnitTests.Fakes;

public class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string password) => $"HASH:{password}";
    public bool Verify(string password, string passwordHash) => passwordHash == $"HASH:{password}";
}
