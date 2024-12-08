namespace Silverfly.Nodes.Operators;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class BinaryOperatorNode : AstNode
{
    public BinaryOperatorNode(AstNode leftExpr, Token @operator, AstNode rightExpr)
    {
        Properties.Set(nameof(Operator), @operator);
        Children.Add(leftExpr);
        Children.Add(rightExpr);
    }

    public Token Operator => Properties.GetOrThrow<Token>(nameof(Operator));
    public AstNode Left => Children.First;
    public AstNode Right => Children.Last;
}
