using System.Collections.Immutable;
using Silverfly;
using Silverfly.Nodes;

namespace Sample.Nodes;

public record VariableBindingNode(Token Name, ImmutableList<NameNode> Parameters, AstNode Value) : StatementNode
{

}