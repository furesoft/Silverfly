using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class BooleanLiteralParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode<bool>(bool.Parse(token.Text.Span)).WithRange(token);
    }
}
