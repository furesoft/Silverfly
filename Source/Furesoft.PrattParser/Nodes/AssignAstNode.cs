namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// An assignment expression like "a = b"
/// </summary>
public class AssignAstNode : AstNode
{
    public string Name { get; }
    public AstNode ValueExpr { get; }

    public AssignAstNode(string name, AstNode valueExpr)
    {
        Name = name;
        ValueExpr = valueExpr;
    }
}
