namespace Silverfly.Sample.Rockstar.Evaluation;

public class EvaluationContext
{
    public Stack<object> Stack = new();
    public required Scope Scope;
}
