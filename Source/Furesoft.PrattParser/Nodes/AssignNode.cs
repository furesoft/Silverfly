namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// An assignment expression like "a = b"
/// </summary>
public class AssignNode : AstNode
{
    public string Name { get; }
    public AstNode ValueExpr { get; }

    public AssignNode(string name, AstNode valueExpr)
    {
        Name = name;
        ValueExpr = valueExpr;
    }
}
