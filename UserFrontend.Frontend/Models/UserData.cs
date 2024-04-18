using System.Security.Claims;

namespace UserFrontend.Frontend.Models;

public sealed class UserData
{
    public string CompanyName { get; set; }
    public string Level { get; set; }
    public string Street { get; set; }
    public string City { get; set; }

    public ClaimsPrincipal ToClaimsPrincipal() =>
        new(new ClaimsIdentity(
        [
            new(ClaimTypes.NameIdentifier, CompanyName),
            new(nameof(Level), Level),
            new(nameof(Street), Street),
            new(nameof(City), City),
        ], "Bearer"
        ));

    public static UserData FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal) => new()
    {
        CompanyName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
        Level = claimsPrincipal.FindFirst("level")?.Value ?? "",
        Street = claimsPrincipal.FindFirst("street")?.Value ?? "",
        City = claimsPrincipal.FindFirst("city")?.Value ?? "",
    };
}
