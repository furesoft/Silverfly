using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Sample.FuncLanguage.Nodes;

public record AnnotatedNode() : AstNode
{
    public ImmutableList<CallNode> Annotations { get; set; } = [];
}