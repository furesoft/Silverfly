using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet to parse a function call like "a(b, c, d)".
/// </summary>
public class CallParselet : IInfixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, AstNode left, Token token)
    {
        // Parse the comma-separated arguments until we hit, ')'.
        var args = parser.ParseSeperated(PredefinedSymbols.Comma, PredefinedSymbols.RightParen);

        return new CallAstNode(left, args).WithRange(left.Range.Start, parser.Prev().GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Call;
    }
}
