namespace Silverfly.Sample.Func.Values;

public record ListValue(List<Value> Value) : Value
{
    public override bool IsTruthy() => true;

    protected override Value GetByIndex(int index)
    {
        if (index >= 0 && index < Value.Count)
        {
            return Value[index];
        }

        return UnitValue.Shared;
    }

    public override string ToString()
    {
        return $"[{string.Join(',', Value)}]";
    }
}
