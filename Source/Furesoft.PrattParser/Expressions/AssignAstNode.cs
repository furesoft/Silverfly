using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// An assignment expression like "a = b"
/// </summary>
public class AssignAstNode : IAstNode {
   private readonly string _name;
   private readonly IAstNode _valueExpr;

   public AssignAstNode(string name, IAstNode valueExpr) {
      _name = name;
      _valueExpr = valueExpr;
   }

   public void Print(StringBuilder sb) {
      sb.Append('(').Append(_name).Append(" = ");
      _valueExpr.Print(sb);
      sb.Append(')');
   }
}