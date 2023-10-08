namespace Furesoft.PrattParser.Nodes.Operators;

public class TernaryOperatorNode(AstNode firstExpr, AstNode secondExpr, AstNode thirdExpr) : AstNode
{
    public AstNode FirstExpr { get; } = firstExpr;
    public AstNode SecondExpr { get; } = secondExpr;
    public AstNode ThirdExpr { get; } = thirdExpr;
}
