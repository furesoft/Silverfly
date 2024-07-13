using Silverfly.Parselets;
using Silverfly.Nodes;
using System.Collections.Immutable;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

public class VariableBindingParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // let name parameter* = value
        var name = parser.Consume(PredefinedSymbols.Name);
        var parameters = parser.ParseList(bindingPower: 0, "=");

        var value = parser.Parse(0);

        return new VariableBindingNode(name, parameters.Cast<NameNode>().ToImmutableList(), value).WithRange(name, parser.LookAhead(0));
    }
}
