using System.Collections.Generic;

namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallNode : AstNode
{
    public AstNode FunctionExpr { get; }
    public List<AstNode> ArgumentExprs { get; }

    public CallNode(AstNode functionExpr, List<AstNode> argumentExpressions)
    {
        FunctionExpr = functionExpr;
        ArgumentExprs = argumentExpressions;
    }
}
