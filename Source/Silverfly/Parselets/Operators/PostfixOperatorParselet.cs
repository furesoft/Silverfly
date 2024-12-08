using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Parselets.Operators;

/// <summary>
/// Generic infix parselet for an unary arithmetic operator.
/// </summary>
public class PostfixOperatorParselet(int bindingPower, string tag) : IInfixParselet
{

    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        return new PostfixOperatorNode(left, token)
            .WithTag(tag)
            .WithRange(left.Range.Document, left.Range.Start, token.GetSourceSpanEnd());
    }

    public int GetBindingPower() => bindingPower;
}
