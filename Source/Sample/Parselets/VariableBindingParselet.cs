using Furesoft.PrattParser;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Nodes;
using Sample.Nodes;
using System.Collections.Immutable;

namespace Sample.Parselets;

public class VariableBindingParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // let name = value
        var name = parser.Consume(PredefinedSymbols.Name);
        var parameters = parser.ParseList("=");

        parser.Consume(PredefinedSymbols.Equals);

        var value = parser.Parse(0);

        return new VariableBindingNode(name, parameters.Cast<NameNode>().ToImmutableList(), value).WithRange(name, parser.LookAhead(0));
    }
}
