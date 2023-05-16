using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet for the condition or "ternary" operator, like "a ? b : c".
/// </summary>
public class ConditionalParselet : IInfixParselet<IAstNode>
{
    public IAstNode Parse(Parser<IAstNode> parser, IAstNode left, Token token)
    {
        var thenArm = parser.Parse();
        parser.Consume(PredefinedSymbols.Colon);

        var elseArm = parser.Parse((int)BindingPower.Conditional - 1);

        return new ConditionalAstNode(left, thenArm, elseArm);
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Conditional;
    }
}
