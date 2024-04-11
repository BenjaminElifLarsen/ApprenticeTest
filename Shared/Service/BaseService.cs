namespace Shared.Service;

public abstract class BaseService
{
    protected string _identifier { get; private set; }

    protected BaseService()
    {
        _identifier = GetType().Name;
    }
}
