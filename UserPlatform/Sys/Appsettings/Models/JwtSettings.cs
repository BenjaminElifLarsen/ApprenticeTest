namespace UserPlatform.Sys.Appsettings.Models;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public string RefreshKey { get; set; }
}
