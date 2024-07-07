namespace Sample.FuncLanguage.Values;

public record NameValue(string Name) : Value
{
    public override bool IsTruthy() => true;
}
