namespace Silverfly.Sample.Func.Values;

public record StringValue : Value
{
    public readonly string Value;

    public StringValue(string value)
    {
        Value = value;

        Members.Define("length", value.Length);

        Members.Define("'+", Concat);
    }

    private Value Concat(Value left, Value right) => $"{left}{right}";

    protected override Value GetByIndex(int index)
    {
        if (index >= 0 && index < Value.Length)
        {
            return Value[index];
        }

        return OptionValue.None;
    }

    protected override Value GetByRange(RangeValue range) => Value.Substring(range.Start, range.End - range.Start);

    public override bool IsTruthy() => string.IsNullOrEmpty(Value);

    public override string ToString() => Value.ToString();
}
