using Furesoft.PrattParser;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Nodes;

namespace Sample.Parselets;

public class UnitValueParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(UnitValue.Shared);
    }
}
