namespace Furesoft.PrattParser.Nodes;

public record LiteralNode<T>(T Value) : AstNode
{
}
