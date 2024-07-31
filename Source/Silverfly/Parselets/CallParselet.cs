using Silverfly.Nodes;

namespace Silverfly.Parselets;

/// <summary>
/// Parselet to parse a function call like "a(b, c, d)".
/// </summary>
public class CallParselet(int bindingPower) : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        // Parse the comma-separated arguments until we hit ')'.
        var args = parser.ParseSeperated(PredefinedSymbols.Comma, PredefinedSymbols.RightParen);

        var call = new CallNode(left, args)
            .WithRange(left, parser.LookAhead(0));

        left = left.WithParent(call);

        return (CallNode)call with
        {
            FunctionExpr = left
        };
    }

    public int GetBindingPower() => bindingPower;
}
