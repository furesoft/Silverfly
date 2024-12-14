namespace Silverfly.Sample.Func.Values;

public record BoolValue(bool Value) : Value
{
    public override string ToString()
    {
        return Value.ToString();
    }

    public override bool IsTruthy()
    {
        return Value;
    }
}
