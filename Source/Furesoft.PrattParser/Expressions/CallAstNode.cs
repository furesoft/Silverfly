using System.Collections.Generic;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallAstNode : IAstNode {
    public IAstNode FunctionExpr { get; }
    public List<IAstNode> ArgumentExprs { get; }

   public CallAstNode(IAstNode functionExpr, List<IAstNode> argumentExpressions) {
      FunctionExpr = functionExpr;
      ArgumentExprs = argumentExpressions;
   }
}
