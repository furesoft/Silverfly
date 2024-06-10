using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Furesoft.PrattParser.Parselets.Operators;

/// <summary>
/// Generic prefix parselet for an unary arithmetic operator.
/// </summary>
public class PrefixOperatorParselet(int bindingPower) : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // To handle right-associative operators like "^", we allow a slightly
        // lower binding power when parsing the right-hand side. This will let a
        // parselet with the same bindingPower appear on the right, which will then
        // take *this* parselet's result as its left-hand argument.
        var right = parser.Parse(bindingPower);

        var node = new PrefixOperatorNode(token.Type, right)
                .WithRange(token.Document, token.GetSourceSpanStart(), right.Range.End);

        right.WithParent(node);

        return node;
    }
}
