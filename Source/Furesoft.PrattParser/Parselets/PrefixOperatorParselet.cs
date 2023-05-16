using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Generic prefix parselet for an unary arithmetic operator. Parses prefix unary "-", "+", "~", and "!" expressions.
/// </summary>
public class PrefixOperatorParselet : IPrefixParselet<IAstNode>
{
    private readonly int _bindingPower;

    public PrefixOperatorParselet(int bindingPower)
    {
        _bindingPower = bindingPower;
    }

    public IAstNode Parse(Parser<IAstNode> parser, Token token)
    {
        // To handle right-associative operators like "^", we allow a slightly
        // lower binding power when parsing the right-hand side. This will let a
        // parselet with the same bindingPower appear on the right, which will then
        // take *this* parselet's result as its left-hand argument.
        var right = parser.Parse(_bindingPower);

        return new PrefixOperatorAstNode(token.Type, right);
    }

    public int GetBindingPower()
    {
        return _bindingPower;
    }
}
