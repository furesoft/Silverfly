namespace Furesoft.PrattParser.Expressions;

public class Literal : IAstNode
{
    public object Value { get; }

    public Literal(object value)
    {
        Value = value;
    }
}
