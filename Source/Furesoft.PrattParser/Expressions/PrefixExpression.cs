using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixExpression : IExpression {
   private Symbol _operator;
   private IExpression _rightExpr;

   public PrefixExpression(Symbol op, IExpression rightExpr) {
      _operator = op;
      this._rightExpr = rightExpr;
   }

   public void Print(StringBuilder sb) {
      sb.Append('(').Append(_operator.Punctuator());
      _rightExpr.Print(sb);
      sb.Append(')');
   }
}
