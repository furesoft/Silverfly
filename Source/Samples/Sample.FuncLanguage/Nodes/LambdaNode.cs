using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record LambdaNode(ImmutableList<NameNode> Parameters, AstNode Value) : AstNode
{
}
