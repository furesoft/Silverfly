using System.Collections.Immutable;
using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

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

        return new LambdaNode(p.ToImmutableList(), value).WithRange(parser.Document, parameters.Range.Start, parser.LookAhead().GetSourceSpanEnd());
    }

    public int GetBindingPower() => 100;
}
