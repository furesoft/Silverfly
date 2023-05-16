namespace Furesoft.PrattParser.Nodes;

public class LiteralNode : AstNode
{
    public object Value { get; }

    public LiteralNode(object value)
    {
        Value = value;
    }
}
