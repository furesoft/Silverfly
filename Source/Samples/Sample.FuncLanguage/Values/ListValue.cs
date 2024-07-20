
namespace Silverfly.Sample.Func.Values;

public record ListValue : Value
{
    public List<Value> value { get; }

    public ListValue(List<Value> Value)
    {
        this.value = Value;

        Members.Define("length", value.Count);
    }

    public override bool IsTruthy() => true;

    protected override Value GetByIndex(int index)
    {
        if (index >= 0 && index < value.Count)
        {
            return value[index];
        }

        return OptionValue.None;
    }

    public override string ToString()
    {
        return $"[{string.Join(',', value)}]";
    }
}
