namespace Silverfly.Nodes.Operators;

/// <summary>
///     A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixOperatorNode : AstNode
{
    public PrefixOperatorNode(Token @operator, AstNode expr)
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
