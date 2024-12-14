using Sample.Brainfuck.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Brainfuck.Parselets;

public class LoopParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var loopNode = new LoopNode();
        var instructions = parser.ParseList(terminators: "]");
        loopNode.Children.Add(instructions);

        return loopNode.WithRange(token, parser.LookAhead(0));
    }
}
