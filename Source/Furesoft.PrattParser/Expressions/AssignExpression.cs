using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// An assignment expression like "a = b"
/// </summary>
public class AssignExpression : IExpression {
   private readonly string _name;
   private readonly IExpression _valueExpr;

   public AssignExpression(string name, IExpression valueExpr) {
      _name = name;
      _valueExpr = valueExpr;
   }

   public void Print(StringBuilder sb) {
      sb.Append('(').Append(_name).Append(" = ");
      _valueExpr.Print(sb);
      sb.Append(')');
   }
}