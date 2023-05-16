using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

public class IntegerLiteralParselet : IPrefixParselet<IAstNode>
{
    public IAstNode Parse(Parser<IAstNode> parser, Token token)
    {
        return new Literal(int.Parse(token.Text));
    }
}
