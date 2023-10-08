using System.Collections.Generic;

namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallNode(AstNode functionExpr, List<AstNode> argumentExpressions) : AstNode
{
    public AstNode FunctionExpr { get; } = functionExpr;
    public List<AstNode> ArgumentExprs { get; } = argumentExpressions;
}
