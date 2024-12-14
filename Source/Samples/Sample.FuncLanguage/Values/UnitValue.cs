namespace Silverfly.Sample.Func.Values;

public record UnitValue : Value
{
    public static readonly Value Shared = new UnitValue();

    public override bool IsTruthy()
    {
        return false;
    }

    public override string ToString()
    {
        return "()";
    }
}
