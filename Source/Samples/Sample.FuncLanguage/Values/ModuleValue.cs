namespace Sample.FuncLanguage.Values;

public record ModuleValue : Value
{
    public ModuleValue(Scope scope)
    {
        Members = scope;
    }

    public override bool IsTruthy() => true;
}
