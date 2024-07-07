using Silverfly.Nodes;

namespace Sample.FuncLanguage.Values;

public record ModuleValue(Scope Scope) : Value, IObject
{
    public Value Get(Value key)
    {
        if (key is not NameValue name) return UnitValue.Shared;

        return Scope.Get(name.Name);
    }

    public override bool IsTruthy() => true;

    public void Set(Value key, Value value)
    {
        throw new NotImplementedException();
    }
}
