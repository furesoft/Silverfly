using System.Collections.Generic;
using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallAstNode : IAstNode {
   private IAstNode _functionExpr;
   private List<IAstNode> _argumentExprs;

   public CallAstNode(IAstNode functionExpr, List<IAstNode> argumentExpressions) {
      _functionExpr = functionExpr;
      _argumentExprs = argumentExpressions;
   }

   public void Print(StringBuilder sb) {
      _functionExpr.Print(sb);
      sb.Append('(');
      for (var i = 0; i < _argumentExprs.Count; i++) {
         if (i > 0) sb.Append(", ");
         _argumentExprs[i].Print(sb);
      }
      sb.Append(')');
   }
}