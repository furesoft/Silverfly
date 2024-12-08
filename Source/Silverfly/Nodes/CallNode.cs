using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Silverfly.Nodes;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public class CallNode : AstNode
{
    public CallNode(AstNode functionExpr, ImmutableList<AstNode> arguments)
    {
        Children.Add(functionExpr);
        Children.Add(arguments);
    }

    public AstNode FunctionExpr => Children.First;
    public IEnumerable<AstNode> Arguments => Children.Skip(1);
}
