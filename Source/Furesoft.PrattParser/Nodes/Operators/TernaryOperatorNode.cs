namespace Furesoft.PrattParser.Nodes.Operators;

public class TernaryOperatorNode : AstNode
{
    public AstNode FirstExpr { get; }
    public AstNode SecondExpr { get; }
    public AstNode ThirdExpr { get; }

    public TernaryOperatorNode(AstNode firstExpr, AstNode secondExpr, AstNode thirdExpr)
    {
        FirstExpr = firstExpr;
        SecondExpr = secondExpr;
        ThirdExpr = thirdExpr;
    }
}
