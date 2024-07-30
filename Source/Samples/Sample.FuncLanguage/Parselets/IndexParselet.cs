using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Parselets;

namespace Silverfly.Sample.Func.Parselets;

public class IndexParselet(int bindingPower) : IInfixParselet
{

    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        var expr = parser.Parse(0);
        parser.Consume(PredefinedSymbols.RightSquare);

        return new BinaryOperatorNode(left, ".", expr);
    }

    public int GetBindingPower() => bindingPower;
}
