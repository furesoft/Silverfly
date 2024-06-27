namespace Sample;

public abstract record Value();

public record NumberValue(double Value) : Value;

public record FuncValue(Delegate Value) : Value;

public record UnitValue() : Value
{
    public static readonly Value Shared = new UnitValue();
}

public record BoolValue(bool Value) : Value;