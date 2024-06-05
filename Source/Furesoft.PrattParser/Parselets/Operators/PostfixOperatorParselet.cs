using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Furesoft.PrattParser.Parselets.Operators;

/// <summary>
/// Generic infix parselet for an unary arithmetic operator.
/// </summary>
public class PostfixOperatorParselet(int bindingPower) : IInfixParselet
{
    private readonly int _bindingPower = bindingPower;

    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        return new PostfixOperatorNode(left, token.Type)
                .WithRange(left.Range.Document, left.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower() => _bindingPower;
}
