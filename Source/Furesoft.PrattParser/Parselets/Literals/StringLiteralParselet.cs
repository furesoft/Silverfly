using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class StringLiteralParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new LiteralNode<string>(token.Text.ToString()).WithRange(token);
    }
}
