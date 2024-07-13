namespace Sample.FuncLanguage.Values;

public record NumberValue : Value
{
    public double Value { get; set; }
    public NumberValue(double value)
    {
        Value = value;

        Members.Define("'+", Add);
        Members.Define("'-", Sub);
        Members.Define("'*", Mul);
        Members.Define("'/", Div);
    }

    public override bool IsTruthy() => Value != 0;

    public override string ToString() => Value.ToString();

    private static Value Add(Value left, Value right) => ((NumberValue)left).Value + ((NumberValue)right).Value;
    private static Value Sub(Value left, Value right) => ((NumberValue)left).Value - ((NumberValue)right).Value;
    private static Value Mul(Value left, Value right) => ((NumberValue)left).Value * ((NumberValue)right).Value;
    private static Value Div(Value left, Value right) => ((NumberValue)left).Value / ((NumberValue)right).Value;
}
