using Silverfly.Nodes;

namespace Silverfly.Parselets.Literals;

public class StringLiteralParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(token.Text.ToString(), token).WithRange(token);
    }
}
