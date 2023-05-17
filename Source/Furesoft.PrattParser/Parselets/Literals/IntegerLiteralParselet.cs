using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class IntegerLiteralParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new LiteralNode(int.Parse(token.Text)).WithRange(token);
    }
}
