using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Parselets.Operators;

/// <summary>
/// Generic prefix parselet for an unary arithmetic operator.
/// </summary>
public class PrefixOperatorParselet(int bindingPower, string tag) : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // To handle right-associative operators like "^", we allow a slightly
        // lower binding power when parsing the right-hand side. This will let a
        // parselet with the same bindingPower appear on the right, which will then
        // take *this* parselet's result as its left-hand argument.
        var right = parser.Parse(bindingPower);

        return new PrefixOperatorNode(token, right)
            .WithTag(tag)
            .WithRange(token.Document, token.GetSourceSpanStart(), right.Range.End);
    }
}
