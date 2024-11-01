using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Brainfuck.Parselets;

public class LoopParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var instructions = parser.ParseList(terminators: "]");

        return new BlockNode(null, "]")
            .WithChildren(instructions)
            .WithTag("loop")
            .WithRange(token, parser.LookAhead());
    }
}
