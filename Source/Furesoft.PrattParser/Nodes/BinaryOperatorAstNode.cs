namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class BinaryOperatorAstNode : AstNode
{
    public AstNode LeftExpr { get; }
    public Symbol Operator { get; }
    public AstNode RightExpr { get; }

    public BinaryOperatorAstNode(AstNode leftExpr, Symbol op, AstNode rightExpr)
    {
        LeftExpr = leftExpr;
        Operator = op;
        RightExpr = rightExpr;
    }
}
