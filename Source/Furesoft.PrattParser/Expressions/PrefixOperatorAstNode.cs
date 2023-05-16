using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A prefix unary arithmetic expression like "!a" or "-b".
/// </summary>
public class PrefixOperatorAstNode : IAstNode {
    public Symbol Operator { get; }
    public IAstNode Expr { get; }

   public PrefixOperatorAstNode(Symbol op, IAstNode rightExpr) {
      Operator = op;
      this.Expr = rightExpr;
   }
}
