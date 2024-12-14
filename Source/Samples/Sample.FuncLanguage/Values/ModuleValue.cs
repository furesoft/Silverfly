namespace Silverfly.Sample.Func.Values;

public record ModuleValue : Value
{
    public ModuleValue(Scope scope)
    {
        Members = scope;
    }

    public override bool IsTruthy()
    {
        return true;
    }

    public override string ToString()
    {
        if (Members.TryGet<LambdaValue>("to_string", out var func))
        {
            return func.Invoke().ToString();
        }

        return $"Module with {Members.Bindings.Count} Bindings";
    }
}
