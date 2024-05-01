namespace UserFrontend.Frontend.Models;

public class ResponseCarrier<T>
{
    public T Data { get; private set; }
    public ErrorResponse Error { get; private set; }

    public ResponseCarrier(T data)
    {
        Data = data;
        Error = new ErrorResponse();
    }

    public ResponseCarrier(ErrorResponse errors)
    {
        Error = errors;
        Data = default!;        
    }
}

public class ResponseCarrier : ResponseCarrier<object>
{
    public ResponseCarrier() : base(null!)
    {
    }

    public ResponseCarrier(ErrorResponse error) : base(error)
    {
    }
}
