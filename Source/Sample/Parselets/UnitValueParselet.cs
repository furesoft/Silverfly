using Silverfly;
using Silverfly.Parselets;
using Silverfly.Nodes;

namespace Sample.Parselets;

public class UnitValueParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(UnitValue.Shared);
    }
}
