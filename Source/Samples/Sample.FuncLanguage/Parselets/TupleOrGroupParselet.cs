using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

public class TupleOrGroupParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var values = parser.ParseSeperated(",", ")");

        if (values.Count == 1)
        {
            return new GroupNode("(", ")", values[0]).WithRange(token, parser.LookAhead(0));
        }

        return new TupleNode(values).WithRange(token, parser.LookAhead(0));
    }
}
