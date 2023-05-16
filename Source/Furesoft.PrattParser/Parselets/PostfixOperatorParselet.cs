using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Generic infix parselet for an unary arithmetic operator. Parses
/// postfix unary "?" expressions.
/// </summary>
public class PostfixOperatorParselet : IInfixParselet<IAstNode>
{
    private readonly int _bindingPower;

    public PostfixOperatorParselet(int bindingPower)
    {
        _bindingPower = bindingPower;
    }

    public IAstNode Parse(Parser<IAstNode> parser, IAstNode left, Token token)
    {
        return new PostfixOperatorAstNode(left, token.Type);
    }

    public int GetBindingPower()
    {
        return _bindingPower;
    }
}
