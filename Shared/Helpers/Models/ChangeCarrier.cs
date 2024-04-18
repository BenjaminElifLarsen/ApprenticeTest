namespace Shared.Helpers.Models;

public sealed class ChangeCarrier<T>
{
    public T Data { get; private set; }

    public ChangeCarrier(T data)
    {
        Data = data;
    }
}
