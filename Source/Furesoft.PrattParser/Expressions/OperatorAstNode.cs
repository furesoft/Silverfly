using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class OperatorAstNode : IAstNode {
   private IAstNode _leftExpr;
   private Symbol _operator;
   private IAstNode _rightExpr;

   public OperatorAstNode(IAstNode leftExpr, Symbol op, IAstNode rightExpr) {
      _leftExpr = leftExpr;
      _operator = op;
      _rightExpr = rightExpr;
   }

   public void Print(StringBuilder sb) {
      sb.Append('(');
      _leftExpr.Print(sb);
      sb.Append(' ').Append(_operator.Punctuator()).Append(' ');
      _rightExpr.Print(sb);
      sb.Append(')');
   }
}
