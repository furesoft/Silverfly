using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record VariableBindingNode(Token Name, ImmutableList<NameNode> Parameters, AstNode Value) : AnnotatedNode
{
}
