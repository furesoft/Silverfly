using System.Collections.Immutable;
using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;
using static Silverfly.PredefinedSymbols;

namespace Silverfly.Sample.Func.Parselets;

public class VariableBindingParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // let name (, name)* = value
        var names = new List<Token>();
        while (true)
        {
            var name = parser.Consume(Name);
            names.Add(name);
            if (parser.LookAhead(0).Type != ",")
            {
                break;
            }

            parser.Consume(",");
        }

        if (names.Count == 1 && parser.LookAhead(0).Type == "=")
        {
            parser.Consume("=");

            // No parameter with single name
            var value = parser.Parse(0);
            return new VariableBindingNode(names[0], [], value).WithRange(names[0], parser.LookAhead(0));
        }

        if (names.Count > 1 && parser.LookAhead(0).Type == "=")
        {
            parser.Consume("=");

            // Tuple-Destructuring
            var value = parser.Parse(0);

            return new TupleBindingNode([.. names.Select(name => new NameNode(name))], value)
                .WithRange(names[0], parser.LookAhead(0));
        }
        else
        {
            // with parameters
            var parameters = parser.ParseList(0, "=");
            var value = parser.Parse(0);

            return new VariableBindingNode(names[0], parameters.Cast<NameNode>().ToImmutableList(), value).WithRange(
                names[0], parser.LookAhead(0));
        }
    }
}
