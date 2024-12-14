namespace Silverfly.Sample.Rockstar.Evaluation;

public class EvaluationContext
{
    public required Scope Scope;
    public Stack<object> Stack = new();
}
