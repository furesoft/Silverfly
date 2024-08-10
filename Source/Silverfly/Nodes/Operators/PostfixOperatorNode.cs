namespace Silverfly.Nodes.Operators;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public record PostfixOperatorNode(AstNode Expr, Token Operator, string Tag) : AstNode
{
}
