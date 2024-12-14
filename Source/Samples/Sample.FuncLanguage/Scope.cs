using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func;

#nullable enable

public class Scope(bool isRoot = false)
{
    public static readonly Scope Root = new(true);
    public Scope? Parent { get; set; }
    public bool IsRoot { get; set; } = isRoot;
    public Dictionary<string, Value> Bindings { get; set; } = [];

    public Scope NewSubScope()
    {
        return new Scope { Parent = this };
    }

    public bool TryGet(string name, out Value value)
    {
        return Bindings.TryGetValue(name, out value!);
    }

    public bool TryGet<T>(string name, out T value)
        where T : Value
    {
        var result = Bindings.TryGetValue(name, out var tmp);

        value = (T)tmp!;
        return result;
    }

    public void Define(string name, Value value)
    {
        Bindings[name] = value;
    }

    public void Define(string name, Delegate value)
    {
        Define(name, Value.From(value));
    }

    public Value? Get(string name)
    {
        if (Bindings.TryGetValue(name, out var value))
        {
            return value;
        }

        return Parent?.Get(name)!;
    }

    public T? Get<T>(string name)
        where T : Value
    {
        return (T)Get(name)!;
    }
}
