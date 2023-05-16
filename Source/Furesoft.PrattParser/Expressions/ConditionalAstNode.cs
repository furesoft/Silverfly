namespace Furesoft.PrattParser.Expressions;

public class ConditionalAstNode : IAstNode {
    public IAstNode ConditionExpr { get; }
    public IAstNode ThenExpr { get; }
    public IAstNode ElseExpr { get; }

   public ConditionalAstNode(IAstNode conditionExpr, IAstNode thenExpr, IAstNode elseExpr) {
      ConditionExpr = conditionExpr;
      ThenExpr = thenExpr;
      ElseExpr = elseExpr;
   }
}
