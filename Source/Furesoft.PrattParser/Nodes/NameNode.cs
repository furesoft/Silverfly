namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public record NameNode(string Name) : AstNode
{
}
