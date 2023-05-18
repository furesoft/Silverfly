namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// An assignment expression like "a = b"
/// </summary>
public class AssignNode : AstNode
{
    public string Name { get; }
    public AstNode ValueExpr { get; }

    public Symbol Operator { get; set; }

    public AssignNode(string name, Symbol op, AstNode valueExpr)
    {
        Name = name;
        Operator = op;
        ValueExpr = valueExpr;
    }
}
