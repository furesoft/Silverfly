using Silverfly.Nodes;

namespace Silverfly.Parselets.Literals;

public class BooleanLiteralParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(bool.Parse(token.Text.Span), token).WithRange(token);
    }
}
