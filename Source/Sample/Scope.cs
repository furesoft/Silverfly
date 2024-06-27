namespace Sample;

public class Scope
{
    public Scope? Parent { get; set; }
    public Dictionary<string, Func<Value[], Value>> Bindings { get; set; } = [];

    public static readonly Scope Root = new();

    public Scope NewSubScope()
    {
        return new Scope
        {
            Parent = this
        };
    }

    public void Define(string name, Func<object[], object> value)
    {
        Bindings[name] = value;
    }

    public Func<Value[], Value>? Get(string name)
    {
        if (Bindings.TryGetValue(name, out var value))
        {
            return value;
        }

        return Parent?.Get(name);
    }
}