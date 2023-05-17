namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixOperatorNode : AstNode
{
    public AstNode Expr { get; }
    public Symbol Operator { get; }

    public PostfixOperatorNode(AstNode left, Symbol op)
    {
        Expr = left;
        Operator = op;
    }
}
