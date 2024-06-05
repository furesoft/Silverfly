using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Simple parselet for a named variable like "abc".
/// </summary>
public class NameParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new NameNode(token.Text.ToString()).WithRange(token);
    }
}
