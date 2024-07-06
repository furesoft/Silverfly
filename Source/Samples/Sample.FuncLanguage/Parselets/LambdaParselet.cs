using System.Collections.Immutable;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;
using Sample.FuncLanguage.Nodes;

namespace Sample.FuncLanguage.Parselets;

public class LambdaParselet : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode parameters, Token token)
    {
        var p = new List<NameNode>();

        if (parameters is NameNode n)
        {
            p.Add(n);
        }
        else if (parameters is TupleNode t)
        {
            p.AddRange(t.Values.Cast<NameNode>());
        }

        var value = parser.ParseExpression();

        return new LambdaNode(p.ToImmutableList(), value).WithRange(parser.Document, parameters.Range.Start, parser.LookAhead(0).GetSourceSpanEnd());
    }

    public int GetBindingPower() => 100;
}
