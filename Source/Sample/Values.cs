namespace Sample;

public abstract record Value()
{
    public abstract bool IsTruthy();
}

public record NumberValue(double Value) : Value
{
    public override bool IsTruthy() => Value != 0;

    public override string ToString() => Value.ToString();
}

public record UnitValue() : Value
{
    public static readonly Value Shared = new UnitValue();

    public override bool IsTruthy() => false;

    public override string ToString() => "()";
}

public record BoolValue(bool Value) : Value
{
    public override string ToString() => Value.ToString();

    public override bool IsTruthy() => Value;
}
public record ListValue(List<Value> Value) : Value
{
    public override bool IsTruthy() => true;

    public override string ToString()
    {
        return $"[{string.Join(',', Value)}]";
    }
}

public record LambdaValue(Func<Value[], Value> Value) : Value
{
    public override bool IsTruthy() => true;
    public override string ToString()
    {
        var paramCount = Value.Method.GetParameters().Length;
        var paramList = string.Join(',', Enumerable.Repeat("Value", paramCount));

        return $"({paramList}) -> Value";
    }
}
