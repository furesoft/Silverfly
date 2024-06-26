namespace Sample;

public class Scope
{
    public Scope Parent { get; set; }
    public Dictionary<string, object> Bindings { get; set; } = [];

    public static Scope Root = new Scope();

    public Scope NewSubScope()
    {
        return new Scope {
            Parent = this
        };
    }

    public void Define(string name, object value)
    {
        Bindings[name] = value;
    }

    public object Get(string name)
    {
        if (Bindings.ContainsKey(name))
        {
            return Bindings[name];
        }

        return Parent?.Get(name);
    }
}