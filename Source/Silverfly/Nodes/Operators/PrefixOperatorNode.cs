namespace Silverfly.Nodes.Operators;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixOperatorNode : AstNode
{
    public Token Operator => Properties.GetOrThrow<Token>(nameof(Operator));
    public AstNode Expr => Children.First;

    public PrefixOperatorNode(Token @operator, AstNode expr)
    {
        Properties.Set(nameof(Operator), @operator);
        Children.Add(expr);
    }
}
