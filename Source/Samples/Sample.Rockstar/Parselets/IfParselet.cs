using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Rockstar.Nodes;

namespace Silverfly.Sample.Rockstar.Parselets;

public class IfParselet : IStatementParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var condition = parser.ParseExpression();
        var body = parser.ParseList(PredefinedSymbols.EOF, Environment.NewLine + Environment.NewLine);

        return new IfNode(condition, body, []);
    }
}
