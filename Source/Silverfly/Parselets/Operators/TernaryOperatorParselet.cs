using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Parselets.Operators;

/// <summary>
///     Parselet for the condition or "ternary" operator, like "a ? b : c".
/// </summary>
public class TernaryOperatorParselet(Symbol lastSymbol, int bindingPower) : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode firstExpr, Token token)
    {
        var secondExpr = parser.ParseExpression();
        parser.Consume(lastSymbol);

        var thirdExpr = parser.Parse(GetBindingPower() - 1);

        return new TernaryOperatorNode(firstExpr, secondExpr, thirdExpr)
            .WithRange(firstExpr.Range.Document, firstExpr.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower() => bindingPower;
}
