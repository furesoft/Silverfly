using Silverfly;
using Silverfly.Parselets;
using Silverfly.Nodes;
using Sample.FuncLanguage.Nodes;
using System.Collections.Immutable;

namespace Sample.FuncLanguage.Parselets;

public class VariableBindingParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // let name parameter* = value
        var name = parser.Consume(PredefinedSymbols.Name);
        var parameters = parser.ParseList(bindingPower: 0, "=");

        parser.Consume(PredefinedSymbols.Equals);

        var value = parser.Parse(0);

        return new VariableBindingNode(name, parameters.Cast<NameNode>().ToImmutableList(), value).WithRange(name, parser.LookAhead(0));
    }
}
