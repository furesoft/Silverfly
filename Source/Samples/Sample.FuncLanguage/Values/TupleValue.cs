namespace Silverfly.Sample.Func.Values;

public record TupleValue(List<Value> Values) : Value
{
    protected override Value GetByIndex(int index)
    {
        if (index >= 0 && index < Values.Count)
        {
            return Values[index];
        }

        return OptionValue.None;
    }

    public override bool IsTruthy()
    {
        return true;
    }

    public override string ToString()
    {
        return "(" + string.Join(',', Values) + ")";
    }
}
