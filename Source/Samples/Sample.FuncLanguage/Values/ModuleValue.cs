namespace Silverfly.Sample.Func.Values;

public record ModuleValue : Value
{
    public ModuleValue(Scope scope)
    {
        Members = scope;
    }

    public override bool IsTruthy() => true;

    public override string ToString()
    {
        return $"Module with {Members.Bindings.Count} bindings";
    }
}
