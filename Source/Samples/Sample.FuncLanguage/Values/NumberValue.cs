namespace Sample.FuncLanguage.Values;

public record NumberValue(double Value) : Value
{
    public override bool IsTruthy() => Value != 0;

    public override string ToString() => Value.ToString();
}
