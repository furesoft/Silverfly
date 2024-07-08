namespace Sample.FuncLanguage.Values;

public record StringValue : Value
{
    public readonly string Value;

    public StringValue(string value)
    {
        Value = value;

        Members.Define("length", new NumberValue(value.Length));
    }

    protected override Value GetByIndex(int index)
    {
        if (index >= 0 && index < Value.Length)
        {
            return new StringValue(Value[index].ToString());
        }

        return UnitValue.Shared;
    }

    public override bool IsTruthy() => string.IsNullOrEmpty(Value);

    public void Set(Value key, Value value)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $"'{Value}'";
}
