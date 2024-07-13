namespace Silverfly.Sample.Func.Values;

public record TupleValue(List<Value> Values) : Value
{
    protected override Value GetByIndex(int index)
    {
        if (index > 0 && index < Values.Count)
        {
            return Values[index];
        }

        return base.GetByIndex(index);
    }

    public override bool IsTruthy() => true;

    public override string ToString() => "(" + string.Join(',', Values) + ")";
}