namespace Silverfly.Nodes.Operators;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public record PrefixOperatorNode(Token Operator, AstNode Expr) : AstNode
{
}
