using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Rockstar.Nodes;

namespace Silverfly.Sample.Rockstar.Parselets;

public class LoopParselet : IStatementParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var condition = parser.ParseExpression();

        if (parser.IsMatch(",") || parser.IsMatch("\n"))
        {
            parser.Consume();
        }

        var body = parser.ParseList(PredefinedSymbols.EOF, "\n\n", ".");

        return new LoopNode(condition, body);
    }
}
