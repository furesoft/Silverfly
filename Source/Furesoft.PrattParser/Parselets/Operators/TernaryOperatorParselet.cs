using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Furesoft.PrattParser.Parselets.Operators;

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

        var node = new TernaryOperatorNode(firstExpr, secondExpr, thirdExpr)
            .WithRange(firstExpr.Range.Document, firstExpr.Range.Start, token.GetSourceSpanEnd());

        firstExpr.WithParent(node);
        secondExpr.WithParent(node);
        thirdExpr.WithParent(node);

        return node;
    }

    public int GetBindingPower() => bindingPower;
}
