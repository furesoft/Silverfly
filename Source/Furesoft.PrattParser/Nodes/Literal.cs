namespace Furesoft.PrattParser.Nodes;

public class Literal : AstNode
{
    public object Value { get; }

    public Literal(object value)
    {
        Value = value;
    }
}
