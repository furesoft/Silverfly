using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Rockstar.Parselets;

public class EmptyStringParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode("", token).WithRange(token);
    }
}
