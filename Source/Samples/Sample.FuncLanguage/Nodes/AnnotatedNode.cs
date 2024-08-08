using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record AnnotatedNode() : AstNode
{
    public ImmutableList<CallNode> Annotations { get; set; } = [];

    public bool HasAnnotation(string name)
    {
        return Annotations
            .Where(_ => _.FunctionExpr is NameNode)
            .Select(_ => (NameNode)_.FunctionExpr)
            .Any(_ => _.Token.Text.ToString() == name);
    }
}
