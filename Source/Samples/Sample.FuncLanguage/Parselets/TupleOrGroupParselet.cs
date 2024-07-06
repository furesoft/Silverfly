using Sample.FuncLanguage.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.FuncLanguage.Parselets;

public class TupleOrGroupParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var values = parser.ParseSeperated(",", ")");

        parser.Consume(")");

        if (values.Count == 1)
        {
            return new GroupNode("(", ")", values[0]);
        }

        return new TupleNode(values);
    }
}