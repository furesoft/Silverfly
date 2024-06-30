namespace Sample;

public abstract record Value();

public record NumberValue(double Value) : Value
{
    public override string ToString() => Value.ToString();
}

public record UnitValue() : Value
{
    public static readonly Value Shared = new UnitValue();

    public override string ToString() => "()";
}

public record BoolValue(bool Value) : Value
{
    public override string ToString() => Value.ToString();
}
public record ListValue(List<Value> Value) : Value
{
    public override string ToString()
    {
        return $"[{string.Join(',', Value)}]";
    }
}

public record LambdaValue(Func<Value[], Value> Value) : Value
{
    public override string ToString()
    {
        var paramCount = Value.Method.GetParameters().Length;
        var paramList = string.Join(',', Enumerable.Repeat("Value", paramCount));

        return $"({paramList}) -> Value";
    }
}
