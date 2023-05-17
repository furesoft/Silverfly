using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Furesoft.PrattParser.Parselets.Operators;

/// <summary>
/// Parselet for the condition or "ternary" operator, like "a ? b : c".
/// </summary>
public class TernaryOperatorParselet : IInfixParselet<AstNode>
{
    private readonly Symbol _lastSymbol;
    private int _bindingPower;

    public TernaryOperatorParselet(Symbol lastSymbol, int bindingPower)
    {
        _lastSymbol = lastSymbol;
        _bindingPower = bindingPower;
    }

    public AstNode Parse(Parser<AstNode> parser, AstNode firstExpr, Token token)
    {
        var secondExpr = parser.Parse();
        parser.Consume(_lastSymbol);

        var thirdExpr = parser.Parse((int)GetBindingPower() - 1);

        return new TernaryOperatorNode(firstExpr, secondExpr, thirdExpr).WithRange(firstExpr.Range.Document, firstExpr.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return _bindingPower;
    }
}
