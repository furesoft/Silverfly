namespace Silverfly.Sample.Rockstar.Evaluation;

public class Scope(bool isRoot = false)
{
    public Scope? Parent { get; set; }
    public bool IsRoot { get; set; } = isRoot;
    public Dictionary<string, object> Bindings { get; set; } = [];

    public static readonly Scope Root = new(true);

    public Scope NewSubScope()
    {
        return new Scope
        {
            Parent = this
        };
    }

    public bool TryGet(string name, out object value)
    {
        return Bindings.TryGetValue(name, out value!);
    }

    public bool TryGet<T>(string name, out T value)
    {
        var result = Bindings.TryGetValue(name, out var tmp);

        value = (T)tmp!;
        return result;
    }

    public void Define(string name, object value)
    {
        Bindings[name] = value;
    }

    public object? Get(string name)
    {
        if (Bindings.TryGetValue(name, out var value))
        {
            return value;
        }

        return Parent?.Get(name)!;
    }

    public T Get<T>(string name)
    {
        return (T)Get(name);
    }

    public string NewIdentifier()
    {
        return $"__tmp__{Guid.NewGuid():N}";
    }
}
