using CateringDataProcessingPlatform.IPL.Appsettings.Models;
using Microsoft.Extensions.Configuration;

namespace CateringDataProcessingPlatform.IPL.Appsettings;

internal sealed class ConfigurationManager : IConfigurationManager
{
    private IConfigurationBuilder _configurationBuilder;
    private IConfiguration _configuration;
    public ConfigurationManager()
    {
        _configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false);
        _configuration = _configurationBuilder.Build();
    }

    public string GetDatabaseString()
    {
        return _configuration.GetConnectionString("database")!;
    }

    public string GetLogKey()
    {
        return _configuration.GetConnectionString("logKey")!;
    }

    public RabbitData GetRabbit()
    {
        var section = _configuration.GetSection("rabbit")!;
        return section.Get<RabbitData>()!;
    }
}
