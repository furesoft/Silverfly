using Sample.Rockstar.Matchers;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Rockstar.Parselets;

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
