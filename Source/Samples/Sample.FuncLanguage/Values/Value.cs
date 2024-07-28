namespace Silverfly.Sample.Func.Values;

public abstract record Value()
{
    public Scope Members = new();

    protected virtual Value GetByIndex(int index) => UnitValue.Shared;
    protected virtual Value GetByRange(RangeValue range) => UnitValue.Shared;

    public Value Get(Value key)
    {
        if (key is NameValue name)
        {
            return Members.Get(name.Name);
        }
        else if (key is NumberValue index)
        {
            return GetByIndex((int)index.Value);
        }
        else if (key is RangeValue range)
        {
            return GetByRange(range);
        }

        return UnitValue.Shared;
    }

    public abstract bool IsTruthy();

    public virtual void Set(Value key, Value value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator Value(string str) => new StringValue(str);
    public static implicit operator Value(char c) => new StringValue(c.ToString());
    public static implicit operator Value(int c) => new NumberValue(c);
    public static implicit operator Value(double c) => new NumberValue(c);
    public static implicit operator Value(bool c) => new BoolValue(c);

    public object Unmarshal()
    {
        if (this is StringValue s)
        {
            return s.Value;
        }
        else if (this is NumberValue n)
        {
            return n.Value;
        }
        else if (this is BoolValue b)
        {
            return b.Value;
        }

        return null;
    }
}
