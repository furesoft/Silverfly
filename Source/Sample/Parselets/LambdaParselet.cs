using System.Collections.Immutable;
using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Sample.Nodes;

namespace Sample.Parselets;

public class LambdaParselet : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode parameters, Token token)
    {
        var p = new List<NameNode>();

        if (parameters is NameNode n)
        {
            p.Add(n);
        }

        var value = parser.ParseExpression();

        return new LambdaNode(p.ToImmutableList(), value).WithRange(parser.Document, parameters.Range.Start, parser.LookAhead(0).GetSourceSpanEnd());
    }

    public int GetBindingPower() => 100;
}
