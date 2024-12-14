using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func;

public class EvaluationContext
{
    public required Scope Scope;
    public Stack<Value> Stack = new();

    internal EvaluationContext NewSubScope()
    {
        return new EvaluationContext { Stack = Stack, Scope = Scope.NewSubScope() };
    }
}
