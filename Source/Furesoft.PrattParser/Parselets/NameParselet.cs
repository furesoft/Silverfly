using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Simple parselet for a named variable like "abc".
/// </summary>
public class NameParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new NameAstNode(token.Text).WithRange(token);
    }
}
