using Furesoft.PrattParser;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Nodes;
using Sample.Nodes;

namespace Sample.Parselets;

public class VariableBindingParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // let name = value
        var name = parser.Consume(PredefinedSymbols.Name);

        parser.Consume(PredefinedSymbols.Equals);

        var value = parser.Parse(0);

        return new VariableBindingNode(name, value).WithRange(name, parser.LookAhead(0));
    }
}
