namespace Furesoft.PrattParser.Nodes;

public class LiteralNode<T>(T value) : AstNode
{
    public T Value { get; } = value;
}
