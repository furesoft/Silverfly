using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Values;

public record LambdaValue : Value
{
    public Func<Value[], Value> Value { get; init; }

    public LambdaNode Definition { get; init; }

    public LambdaValue(Func<Value[], Value> value, LambdaNode definition, bool addOperators = true)
    {
        Value = value;
        Definition = definition;

        if (!addOperators)
        {
            return;
        }

        Members.Define("'+", new LambdaValue(Combine, definition, false));
    }

    private static Value Combine(Value[] args)
    {
        if (args[0] is LambdaValue firstLambda && args[1] is LambdaValue secondLambda)
        {
            Func<Value[], Value> combinedFunction = x => secondLambda.Value([firstLambda.Value([x[0]])]);

            return new LambdaValue(combinedFunction, firstLambda.Definition, false);
        }

        return UnitValue.Shared;
    }

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

    internal Value Invoke(params Value[] args)
    {
        return (Value)Value.DynamicInvoke([args]);
    }
}
