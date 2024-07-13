using System.Collections.Immutable;
using Silverfly;
using Silverfly.Nodes;

namespace Sample.FuncLanguage.Nodes;

public record VariableBindingNode(Token Name, ImmutableList<NameNode> Parameters, AstNode Value) : AnnotatedNode
{
}