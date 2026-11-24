using Keeply.Domain.Entities;

namespace Keeply.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}
