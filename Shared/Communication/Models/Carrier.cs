namespace Shared.Communication.Models;

public sealed class Carrier
{
    public string? Data { get; set; }
    public CarrierResult Result { get; set; }
    public long Error { get; set; }
}
