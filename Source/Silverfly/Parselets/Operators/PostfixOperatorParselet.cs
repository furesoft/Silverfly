using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Parselets.Operators;

/// <summary>
/// Generic infix parselet for an unary arithmetic operator.
/// </summary>
public class PostfixOperatorParselet(int bindingPower) : IInfixParselet
{
    private readonly int _bindingPower = bindingPower;

    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        var node = new PostfixOperatorNode(left, token)
                .WithRange(left.Range.Document, left.Range.Start, token.GetSourceSpanEnd());

        left.WithParent(node);

        return node;
    }

    public int GetBindingPower() => _bindingPower;
}
