namespace Silverfly.Nodes.Operators;

/// <summary>
///     A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixOperatorNode : ExpressionNode
{
    public PostfixOperatorNode(AstNode expr, Token @operator)
    {
        Properties.Set(nameof(Operator), @operator);
        Children.Add(expr);
    }

    public Token Operator
    {
        get => Properties.GetOrThrow<Token>(nameof(Operator));
    }

    public AstNode Expr
    {
        get => Children.First;
    }
}
