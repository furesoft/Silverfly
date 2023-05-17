namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixOperatorNode : AstNode
{
    public Symbol Operator { get; }
    public AstNode Expr { get; }

    public PrefixOperatorNode(Symbol op, AstNode rightExpr)
    {
        Operator = op;
        Expr = rightExpr;
    }
}
