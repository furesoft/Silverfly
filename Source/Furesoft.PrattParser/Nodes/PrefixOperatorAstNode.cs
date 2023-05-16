namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixOperatorAstNode : AstNode
{
    public Symbol Operator { get; }
    public AstNode Expr { get; }

    public PrefixOperatorAstNode(Symbol op, AstNode rightExpr)
    {
        Operator = op;
        Expr = rightExpr;
    }
}
