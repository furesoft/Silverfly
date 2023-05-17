namespace Furesoft.PrattParser.Nodes;

public class LiteralNode<T> : AstNode
{
    public T Value { get; }

    public LiteralNode(T value)
    {
        Value = value;
    }
}
