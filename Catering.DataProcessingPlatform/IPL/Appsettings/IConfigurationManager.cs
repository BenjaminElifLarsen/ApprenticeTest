using Catering.DataProcessingPlatform.IPL.Appsettings.Models;

namespace Catering.DataProcessingPlatform.IPL.Appsettings;

internal interface IConfigurationManager
{
    public string GetDatabaseString();
    public RabbitData GetRabbit();
    public string GetLogKey();
}
