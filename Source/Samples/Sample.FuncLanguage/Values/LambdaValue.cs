using Sample.FuncLanguage.Nodes;

namespace Sample.FuncLanguage.Values;

public record LambdaValue(Func<Value[], Value> Value, LambdaNode Definition) : Value
{
    public override bool IsTruthy() => true;
    public override string ToString()
    {
        var paramCount = 0;
        if (Definition is null)
        {
            paramCount = Value.Method.GetParameters().Length;
        }
        else
        {
            paramCount = Definition.Parameters.Count;
        }

        var paramList = string.Join(',', Enumerable.Repeat("Value", paramCount));

        return $"({paramList}) -> Value";
    }
}
