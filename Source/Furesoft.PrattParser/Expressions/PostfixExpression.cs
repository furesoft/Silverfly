using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixExpression : IExpression {
   private IExpression _leftExpr;
   private Symbol _operator;

   public PostfixExpression(IExpression left, Symbol op) {
      _leftExpr = left;
      _operator = op;
   }

   public void Print(StringBuilder sb) {
      sb.Append('(');
      _leftExpr.Print(sb);
      sb.Append(_operator.Punctuator()).Append(')');
   }
}
