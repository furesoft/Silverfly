using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Rockstar.Matchers;

namespace Silverfly.Sample.Rockstar.Parselets;

public class AliasedBooleanParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        if (AliasedBooleanMatcher.TrueAliases.Contains(token.Text.ToString()))
        {
            return new LiteralNode(true, token).WithRange(token);
        }
        
        if (AliasedBooleanMatcher.FalseAliases.Contains(token.Text.ToString()))
        {
            return new LiteralNode(false, token).WithRange(token);
        }

        return null;
    }
}
