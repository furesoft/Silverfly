using Sample.Brainfuck.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Brainfuck.Parselets;

public class OperationParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new OperationNode(token).WithRange(token);
    }
}
