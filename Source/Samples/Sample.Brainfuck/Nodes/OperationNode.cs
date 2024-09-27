using Silverfly;
using Silverfly.Nodes;

namespace Sample.Brainfuck.Nodes;

public record OperationNode(Token Token) : AstNode
{
}
