using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet for the condition or "ternary" operator, like "a ? b : c".
/// </summary>
public class ConditionalParselet : IInfixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, AstNode left, Token token)
    {
        var thenArm = parser.Parse();
        parser.Consume(PredefinedSymbols.Colon);

        var elseArm = parser.Parse((int)BindingPower.Conditional - 1);

        return new ConditionalAstNode(left, thenArm, elseArm).WithRange(left.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Conditional;
    }
}
