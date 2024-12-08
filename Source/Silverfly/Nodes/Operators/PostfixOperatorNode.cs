namespace Silverfly.Nodes.Operators;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixOperatorNode : AstNode
{
    public Token Operator => Properties.GetOrThrow<Token>(nameof(Operator));
    public AstNode Expr => Children.First;

    public PostfixOperatorNode(AstNode expr, Token @operator)
    {
        Properties.Set(nameof(Operator), @operator);
        Children.Add(expr);
    }
}
