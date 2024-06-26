namespace Sample;

public class Scope
{
    public Scope? Parent { get; set; }
    public Dictionary<string, Func<object[], object>> Bindings { get; set; } = [];

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

    public Func<object[], object>? Get(string name)
    {
        if (Bindings.TryGetValue(name, out var value))
        {
            return value;
        }

        return Parent?.Get(name);
    }
}