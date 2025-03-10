using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Parselets;

namespace Silverfly.Sample.Func.Parselets;

public class IndexParselet(int bindingPower) : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        var expr = parser.Parse(0);
        parser.Consume("]");

        return new BinaryOperatorNode(left, token.Rewrite("."), expr).WithRange(token, parser.LookAhead());
    }

    public int GetBindingPower()
    {
        return bindingPower;
    }
}
