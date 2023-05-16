using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet to parse a function call like "a(b, c, d)".
/// </summary>
public class CallParselet : IInfixParselet<IAstNode>
{
    public IAstNode Parse(Parser<IAstNode> parser, IAstNode left, Token token)
    {
        // Parse the comma-separated arguments until we hit, ')'.
        var args = parser.ParseSeperated(PredefinedSymbols.Comma, PredefinedSymbols.RightParen);

        return new CallAstNode(left, args);
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Call;
    }
}
