using Sample.JSON.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.JSON.Parselets;

class JsonArrayParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var elements = parser.ParseSeperated(",", "]");

        return new JsonArray(elements);
    }
}
