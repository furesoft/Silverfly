namespace Silverfly.Nodes.Operators;

/// <summary>
/// Represents a ternary operator node in an abstract syntax tree (AST).
/// </summary>
public class TernaryOperatorNode : AstNode
{
    public TernaryOperatorNode(AstNode firstExpr, AstNode secondExpr, AstNode thirdExpr)
    {
        Children.Add(firstExpr);
        Children.Add(secondExpr);
        Children.Add(thirdExpr);
    }

    public AstNode FirstExpr => Children.First;
    public AstNode SecondExpr => Children[1];
    public AstNode ThirdExpr => Children.Last;
}
