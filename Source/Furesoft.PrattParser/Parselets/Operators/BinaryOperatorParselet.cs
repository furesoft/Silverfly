using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Furesoft.PrattParser.Parselets.Operators;

/// <summary>
/// Generic infix parselet for a binary arithmetic operator. The only
/// difference when parsing, "+", "-", "*", "/", and "^" is binding power
/// and associativity, so we can use a single parselet class for all of those.
/// </summary>
public class BinaryOperatorParselet : IInfixParselet<AstNode>
{
    private readonly int _bindingPower;
    private readonly bool _isRight;

    public BinaryOperatorParselet(int bindingPower, bool isRight)
    {
        _bindingPower = bindingPower;
        _isRight = isRight;
    }

    public AstNode Parse(Parser<AstNode> parser, AstNode left, Token token)
    {
        // To handle right-associative operators like "^", we allow a slightly
        // lower binding power when parsing the right-hand side. This will let a
        // parselet with the same binding power appear on the right, which will then
        // take *this* parselet's result as its left-hand argument.
        var right = parser.Parse(_bindingPower - (_isRight ? 1 : 0));

        return new BinaryOperatorAstNode(left, token.Type, right).WithRange(left.Range.Document, left.Range.Start, right.Range.End);
    }

    public int GetBindingPower()
    {
        return _bindingPower;
    }
}
