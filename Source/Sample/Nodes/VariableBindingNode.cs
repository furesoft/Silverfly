using System.Collections.Immutable;
using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;

namespace Sample.Nodes;

public record VariableBindingNode(Token Name, ImmutableList<NameNode> Parameters, AstNode Value) : StatementNode
{

}