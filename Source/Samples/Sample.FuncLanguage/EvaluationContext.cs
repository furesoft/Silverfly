using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func;

public class EvaluationContext
{
    public Stack<Value> Stack = new();
    public required Scope Scope;

    internal EvaluationContext NewSubScope()
    {
        return new EvaluationContext()
        {
            Stack = Stack,
            Scope = Scope.NewSubScope(),
        };
    }
}
