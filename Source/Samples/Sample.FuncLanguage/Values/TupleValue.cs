using Silverfly.Nodes;

namespace Sample.FuncLanguage.Values;

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