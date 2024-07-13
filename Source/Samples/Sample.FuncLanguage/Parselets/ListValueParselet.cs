using Silverfly.Parselets;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Parselets;

public class ListValueParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var items = parser.ParseSeperated(",", bindingPower: 0, "]");

        return new LiteralNode(items)
            .WithRange(token, parser.LookAhead(0));
    }
}
