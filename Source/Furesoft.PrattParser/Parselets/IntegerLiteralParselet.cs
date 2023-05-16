using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

public class IntegerLiteralParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new Literal(int.Parse(token.Text));
    }
}
