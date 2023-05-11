using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// Interface for all expression AST node classes.
/// </summary>
public interface IExpression {
   /// <summary>
   /// Pretty-print the expression to a string.
   /// </summary>
   /// <param name="sb"></param>
   void Print(StringBuilder sb);
}