namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class BinaryOperatorNode : AstNode
{
    public AstNode LeftExpr { get; }
    public Symbol Operator { get; }
    public AstNode RightExpr { get; }

    public BinaryOperatorNode(AstNode leftExpr, Symbol op, AstNode rightExpr)
    {
        LeftExpr = leftExpr;
        Operator = op;
        RightExpr = rightExpr;
    }
}
