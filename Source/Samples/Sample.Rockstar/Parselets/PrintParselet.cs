using System.Collections.Immutable;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Silverfly.Sample.Rockstar.Parselets;

public class PrintParselet : IPrefixParselet
{
    public static readonly string[] Aliases = ["say", "shout", "whisper", "scream"];
    public AstNode Parse(Parser parser, Token token)
    {
        var func = new NameNode(token);
        ImmutableList<AstNode> args = [parser.ParseExpression()];
        
        return new CallNode(func, args).WithRange(token, parser.LookAhead(0));
    }
}
