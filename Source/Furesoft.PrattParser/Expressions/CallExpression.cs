using System.Collections.Generic;
using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallExpression : IExpression {
   private IExpression _functionExpr;
   private List<IExpression> _argumentExprs;

   public CallExpression(IExpression functionExpr, List<IExpression> argumentExpressions) {
      _functionExpr = functionExpr;
      _argumentExprs = argumentExpressions;
   }

   public void Print(StringBuilder sb) {
      _functionExpr.Print(sb);
      sb.Append('(');
      for (int i = 0; i < _argumentExprs.Count; i++) {
         if (i > 0) sb.Append(", ");
         _argumentExprs[i].Print(sb);
      }
      sb.Append(')');
   }
}