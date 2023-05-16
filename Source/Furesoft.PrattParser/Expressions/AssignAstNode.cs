namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// An assignment expression like "a = b"
/// </summary>
public class AssignAstNode : IAstNode {
    public string Name { get; }
    public IAstNode ValueExpr { get; }

   public AssignAstNode(string name, IAstNode valueExpr) {
      Name = name;
      ValueExpr = valueExpr;
   }
}
