using Sample.FuncLanguage.Nodes;
using Silverfly.Nodes;

namespace Sample.FuncLanguage;

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

public record LambdaValue(Func<Value[], Value> Value, LambdaNode Definition) : Value
{
    public override bool IsTruthy() => true;
    public override string ToString()
    {
        var paramCount = 0;
        if (Definition is null)
        {
            paramCount = Value.Method.GetParameters().Length;
        }
        else
        {
            paramCount = Definition.Parameters.Count;
        }

        var paramList = string.Join(',', Enumerable.Repeat("Value", paramCount));

        return $"({paramList}) -> Value";
    }
}

public record TupleValue(List<Value> Values) : Value, IObject
{
    public Value Get(Value key)
    {
        if (key is NumberValue v && v.Value < Values.Count)
        {
            return Values[(int)v.Value];
        }

        return UnitValue.Shared;
    }

    public override bool IsTruthy() => true;

    public void Set(Value key, Value value)
    {
        if (key is NumberValue v)
        {
            Values[(int)v.Value] = value;
        }
    }

    public override string ToString() => "(" + string.Join(',', Values) + ")";
}