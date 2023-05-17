namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public class NameAstNode : AstNode
{
    public NameAstNode(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
