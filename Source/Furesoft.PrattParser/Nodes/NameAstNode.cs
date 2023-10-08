namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public class NameAstNode(string name) : AstNode
{
    public string Name { get; } = name;
}
