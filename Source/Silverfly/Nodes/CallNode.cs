using System.Collections.Immutable;

namespace Silverfly.Nodes;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public record CallNode(AstNode FunctionExpr, ImmutableList<AstNode> Arguments) : AstNode
{
}
