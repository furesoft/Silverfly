namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixOperatorNode(Symbol op, AstNode rightExpr) : AstNode
{
    public Symbol Operator { get; } = op;
    public AstNode Expr { get; } = rightExpr;
}
