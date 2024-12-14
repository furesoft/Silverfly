using Silverfly.Nodes;

namespace Silverfly.Parselets;

/// <summary>
///     Parselet to parse a function call like "a(b, c, d)".
/// </summary>
public class CallParselet(int bindingPower) : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        // Parse the comma-separated arguments until we hit ')'.
        var args = parser.ParseSeperated(",", ")");

        return new CallNode(left, args)
            .WithRange(left, parser.LookAhead(0));
    }

    public int GetBindingPower()
    {
        return bindingPower;
    }
}
