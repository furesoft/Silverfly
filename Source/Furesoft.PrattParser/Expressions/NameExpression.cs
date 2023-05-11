using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public class NameExpression : IExpression {
   public NameExpression(string name) {
      Name = name;
   }

   public string Name {
      get;
   }

   public void Print(StringBuilder sb) {
      sb.Append(Name);
   }
}