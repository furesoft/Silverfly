namespace Silverfly.Nodes.Operators;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public record BinaryOperatorNode(AstNode LeftExpr, Token Operator, AstNode RightExpr) : AstNode
{
}
