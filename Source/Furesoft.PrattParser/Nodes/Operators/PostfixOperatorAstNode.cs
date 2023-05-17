namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixOperatorAstNode : AstNode
{
    public AstNode Expr { get; }
    public Symbol Operator { get; }

    public PostfixOperatorAstNode(AstNode left, Symbol op)
    {
        Expr = left;
        Operator = op;
    }
}
