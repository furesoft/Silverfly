using Sample.JSON.Nodes;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Silverfly.Sample.JSON.Parselets;

internal class JsonArrayParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var elements = parser.ParseSeperated(",", "]");

        return new JsonArray(elements);
    }
}
