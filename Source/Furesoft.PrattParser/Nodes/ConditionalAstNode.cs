namespace Furesoft.PrattParser.Nodes;

public class ConditionalAstNode : AstNode
{
    public AstNode ConditionExpr { get; }
    public AstNode ThenExpr { get; }
    public AstNode ElseExpr { get; }

    public ConditionalAstNode(AstNode conditionExpr, AstNode thenExpr, AstNode elseExpr)
    {
        ConditionExpr = conditionExpr;
        ThenExpr = thenExpr;
        ElseExpr = elseExpr;
    }
}
