using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record AnnotatedNode() : AstNode
{
    public ImmutableList<CallNode> Annotations { get; set; } = [];
}