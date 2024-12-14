using Silverfly.Nodes;

namespace Silverfly.Parselets;

/// <summary>
///     Simple parselet for a named variable like "abc".
/// </summary>
public class NameParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new NameNode(token).WithRange(token);
    }
}
