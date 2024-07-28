using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func;

#nullable enable

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

    public void Define(string name, Func<Value[], Value> value)
    {
        Bindings[name] = new LambdaValue(value, null);
    }

    public void Define(string name, Value value)
    {
        Bindings[name] = value;
    }

    public void Define(string name, Func<Value, Value> value)
    {
        Define(name, args => value(args[0]));
    }

    public void Define(string name, Func<Value, Value, Value> value)
    {
        Define(name, args => value(args[0], args[1]));
    }

    public Value Get(string name)
    {
        if (Bindings.TryGetValue(name, out var value))
        {
            return value;
        }

        return Parent?.Get(name)!;
    }

    public T Get<T>(string name)
        where T : Value
    {
        return (T)Get(name);
    }

    public string NewIdentifier()
    {
        return $"__tmp__{Guid.NewGuid():N}";
    }
}