namespace Shared.Patterns.ResultPattern;

public sealed class BinaryFlag
{
    private long _flag;

    public BinaryFlag()
    {
        _flag = 0;        
    }

    private void AddFlag(long flag)
    {
        if ((_flag & flag) == 0)
            _flag += flag;
    }

    public static BinaryFlag operator +(BinaryFlag left, Enum right)
    {
        left.AddFlag(EnumToLong(right));
        return left;
    }


    public static implicit operator bool(BinaryFlag flag) => flag is not null && flag._flag == 0;

    public override string ToString()
    {
        return Convert.ToString(_flag, 2);
    }



    private static long EnumToLong(Enum e)
    {
        var value = Convert.ChangeType(e, Enum.GetUnderlyingType(e.GetType()));
        return Convert.ToInt64(value);
    }
}
