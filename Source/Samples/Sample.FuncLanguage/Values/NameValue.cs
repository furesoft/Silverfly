namespace Silverfly.Sample.Func.Values;

public record NameValue(string Name) : Value
{
    public override bool IsTruthy() => true;
}
