using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A ternary conditional expression like "a ? b : c".
/// </summary>
public class ConditionalAstNode : IAstNode {
   private IAstNode _conditionExpr;
   private IAstNode _thenExpr;
   private IAstNode _elseExpr;

   public ConditionalAstNode(IAstNode conditionExpr, IAstNode thenExpr, IAstNode elseExpr) {
      _conditionExpr = conditionExpr;
      _thenExpr = thenExpr;
      _elseExpr = elseExpr;
   }

   public void Print(StringBuilder sb) {
      sb.Append('(');
      _conditionExpr.Print(sb);
      sb.Append(" ? ");
      _thenExpr.Print(sb);
      sb.Append(" : ");
      _elseExpr.Print(sb);
      sb.Append(')');
   }
}