using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class SignedIntegerLiteralParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new LiteralNode<int>(int.Parse(token.Text)).WithRange(token);
    }
}
