namespace Sample.FuncLanguage.Values;

public abstract record Value()
{
    public Scope Members = new();

    protected virtual Value GetByIndex(int index) => UnitValue.Shared;

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

        return UnitValue.Shared;
    }

    public abstract bool IsTruthy();

    public void Set(Value key, Value value)
    {
        throw new NotImplementedException();
    }
}
