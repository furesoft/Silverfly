namespace Silverfly.Sample.Func.Values;

public record ListValue : Value
{
    public ListValue(List<Value> Value)
    {
        this.Value = Value;

        Members.Define("length", this.Value.Count);
    }

    public List<Value> Value { get; }

    public override bool IsTruthy()
    {
        return true;
    }

    protected override Value GetByIndex(int index)
    {
        if (index >= 0 && index < Value.Count)
        {
            return Value[index];
        }

        return OptionValue.None;
    }

    protected override Value GetByRange(RangeValue range)
    {
        return new ListValue([.. Value.Skip(range.Start).Take(range.End - range.Start)]);
    }

    public override string ToString()
    {
        return $"[{string.Join(',', Value)}]";
    }
}
