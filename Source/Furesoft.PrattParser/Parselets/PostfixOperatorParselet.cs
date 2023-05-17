using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Generic infix parselet for an unary arithmetic operator. Parses
/// postfix unary "?" expressions.
/// </summary>
public class PostfixOperatorParselet : IInfixParselet<AstNode>
{
    private readonly int _bindingPower;

    public PostfixOperatorParselet(int bindingPower)
    {
        _bindingPower = bindingPower;
    }

    public AstNode Parse(Parser<AstNode> parser, AstNode left, Token token)
    {
        return new PostfixOperatorAstNode(left, token.Type).WithRange(left.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return _bindingPower;
    }
}
