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

        return new CallNode(left, args).WithRange(left.Range.Document, left.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Call;
    }
}
