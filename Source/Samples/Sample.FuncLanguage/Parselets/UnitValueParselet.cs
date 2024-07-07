using Silverfly;
using Silverfly.Parselets;
using Silverfly.Nodes;
using Sample.FuncLanguage.Values;

namespace Sample.FuncLanguage.Parselets;

public class UnitValueParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(UnitValue.Shared);
    }
}
