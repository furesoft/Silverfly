namespace Silverfly.Nodes;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public record NameNode(Token Token) : AstNode
{
}
