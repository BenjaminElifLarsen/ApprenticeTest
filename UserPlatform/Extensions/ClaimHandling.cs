using System.Security.Claims;

namespace UserPlatform.Extensions;

public static class ClaimHandling
{
    public static string GetLevel(this HttpContext context)
    {
        var claims = context.User.Claims;
        var claim = claims.FirstOrDefault(x => x.Type == "level");
        if (claim is null)
            return null!;
        return claim.Value;
    }

    public static Guid GetUserId(this HttpContext context)
    {
        var claims = context.User.Claims;
        var claim = claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        Guid.TryParse(claim?.Value, out var userId);
        return userId;
    }
}
