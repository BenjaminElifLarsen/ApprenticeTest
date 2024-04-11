using CateringDataProcessingPlatform.IPL.Appsettings.Models;

namespace CateringDataProcessingPlatform.IPL.Appsettings;

internal interface IConfigurationManager
{
    public string GetDatabaseString();
    public RabbitData GetRabbit();
    public string GetLogKey();
}
