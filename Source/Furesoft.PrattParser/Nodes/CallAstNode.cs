using System.Collections.Generic;

namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallAstNode : AstNode
{
    public AstNode FunctionExpr { get; }
    public List<AstNode> ArgumentExprs { get; }

    public CallAstNode(AstNode functionExpr, List<AstNode> argumentExpressions)
    {
        FunctionExpr = functionExpr;
        ArgumentExprs = argumentExpressions;
    }
}
