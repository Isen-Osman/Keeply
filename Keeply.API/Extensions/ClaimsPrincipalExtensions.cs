using System.Security.Claims;

namespace Keeply.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue(ClaimTypes.Name)
            ?? user.FindFirstValue("sub");
        return Guid.Parse(value!);
    }
}
