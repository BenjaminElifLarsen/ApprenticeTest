using System.Security.Claims;

namespace UserFrontend.Frontend.Models;

public sealed class UserData
{
    public string CompanyName { get; set; }
    public string Level { get; set; }
    public List<string> Roles { get; set; }

    public ClaimsPrincipal ToClaimsPrincipal() =>
        new(new ClaimsIdentity(
        [
            new(ClaimTypes.NameIdentifier, CompanyName),
            new(nameof(Level), Level),
        ], "Bearer"
        ));

    public static UserData FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal) => new()
    {
        CompanyName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
        Level = claimsPrincipal.FindFirst("level")?.Value ?? "",
    };
}
