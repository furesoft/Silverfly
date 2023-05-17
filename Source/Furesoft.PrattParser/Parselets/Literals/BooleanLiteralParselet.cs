using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class BooleanLiteralParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new LiteralNode(bool.Parse(token.Text)).WithRange(token);
    }
}
