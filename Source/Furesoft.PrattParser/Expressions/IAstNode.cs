using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// Interface for all expression AST node classes.
/// </summary>
public interface IAstNode {
    //public SourceRange Range { get; set; }
   void Print(StringBuilder sb);
}
