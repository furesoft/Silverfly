using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.JSON.Nodes;

namespace Silverfly.Sample.JSON.Parselets;

internal class JsonArrayParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var elements = parser.ParseSeperated(",", "]");

        return new JsonArray(elements);
    }
}
