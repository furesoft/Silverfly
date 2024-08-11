using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

public class IfParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var condition = parser.ParseExpression();
        parser.Consume("then");

        var truePart = parser.ParseExpression();

        parser.Consume("else");

        var falsePart = parser.ParseExpression();

        return new IfNode(condition, truePart, falsePart).WithRange(token, parser.LookAhead(0));
    }
}
