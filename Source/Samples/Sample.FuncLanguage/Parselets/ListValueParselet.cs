using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Silverfly.Sample.Func.Parselets;

public class ListValueParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var items = parser.ParseSeperated(",", 0, "]");

        return new LiteralNode(items, token)
            .WithRange(token, parser.LookAhead(0));
    }
}
