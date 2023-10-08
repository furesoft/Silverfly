namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class BinaryOperatorNode(AstNode leftExpr, Symbol op, AstNode rightExpr) : AstNode
{
    public AstNode LeftExpr { get; } = leftExpr;
    public Symbol Operator { get; } = op;
    public AstNode RightExpr { get; } = rightExpr;
}
