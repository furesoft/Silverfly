using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Sample.FuncLanguage.Nodes;

public record LambdaNode(ImmutableList<NameNode> Parameters, AstNode Value) : AstNode
{
}
