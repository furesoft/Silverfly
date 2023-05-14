using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class OperatorExpression : IExpression {
   private IExpression _leftExpr;
   private Symbol _operator;
   private IExpression _rightExpr;

   public OperatorExpression(IExpression leftExpr, Symbol op, IExpression rightExpr) {
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
