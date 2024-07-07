namespace Sample.FuncLanguage.Values;

public record ListValue(List<Value> Value) : Value
{
    public override bool IsTruthy() => true;

    public override string ToString()
    {
        return $"[{string.Join(',', Value)}]";
    }
}
