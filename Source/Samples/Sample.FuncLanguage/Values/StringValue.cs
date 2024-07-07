namespace Sample.FuncLanguage.Values;

public record StringValue(string Value) : Value, IObject
{
    public Value Get(Value key)
    {
        if (key is NameValue name)
        {
            if (name.Name == "length")
            {
                return new NumberValue(Value.Length);
            }
        }
        else if (key is NumberValue index)
        {
            if (index.Value >= 0 && index.Value < Value.Length)
            {
                return new StringValue(Value[(int)index.Value].ToString());
            }
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
