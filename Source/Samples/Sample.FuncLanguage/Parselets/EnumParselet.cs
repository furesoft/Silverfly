using Silverfly.Nodes;
using Silverfly.Parselets;
using Sivlerfly.Sample.FuncLanguage.Nodes;

namespace Silverfly.Sample.Func.Parselets;

public class EnumParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var name = parser.Consume(PredefinedSymbols.Name);

        parser.Consume("=");
        var members = parser.ParseSeperated("|", bindingPower: 0, PredefinedSymbols.EOL);

        return new EnumNode(name.Text.ToString(), members).WithRange(token, parser.LookAhead(0));
    }
}
