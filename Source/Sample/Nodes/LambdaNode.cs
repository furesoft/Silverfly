using System.Collections.Immutable;
using Furesoft.PrattParser.Nodes;

namespace Sample.Nodes;

public record LambdaNode(ImmutableList<NameNode> Parameters, AstNode Value) : AstNode
{
    
}
