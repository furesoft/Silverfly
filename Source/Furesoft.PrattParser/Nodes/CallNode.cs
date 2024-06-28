using System.Collections.Immutable;

namespace Furesoft.PrattParser.Nodes;

/// <summary>
/// A function call like "a(b, c, d)".
/// </summary>
public record CallNode(AstNode FunctionExpr, ImmutableList<AstNode> Arguments) : AstNode
{

}
