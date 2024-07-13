using Silverfly.Parselets;
using Silverfly.Nodes;
using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func.Parselets;

public class UnitValueParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(UnitValue.Shared);
    }
}
