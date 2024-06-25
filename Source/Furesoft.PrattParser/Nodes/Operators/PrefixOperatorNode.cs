namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public record PrefixOperatorNode(Symbol Operator, AstNode Expr) : AstNode
{
}
