namespace Sample;

public class Scope
{
    public Scope? Parent { get; set; }
    public Dictionary<string, Value> Bindings { get; set; } = [];

    public static readonly Scope Root = new();

    public Scope NewSubScope()
    {
        return new Scope
        {
            Parent = this
        };
    }

    public void Define(string name, Func<Value[], Value> value)
    {
        Bindings[name] = new LambdaValue(value);
    }

    public void Define(string name, Value value)
    {
        Bindings[name] = value;
    }

    public void Define(string name, Func<Value, Value> value)
    {
        Define(name, args => value(args[0]));
    }

    public Value? Get(string name)
    {
        if (Bindings.TryGetValue(name, out var value))
        {
            return value;
        }

        return Parent?.Get(name);
    }
}