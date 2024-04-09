namespace Shared.DDD;

public sealed record ReferenceId : ValueObject
{
    private Guid _id;
    public Guid Id { get => _id; private set => _id = value; }

    private ReferenceId()
    {
        
    }

    public ReferenceId(Guid id)
    {
        _id = id;        
    }
}
