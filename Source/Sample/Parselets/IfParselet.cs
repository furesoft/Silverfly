using Sample.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Parselets;

public class IfParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var condition = parser.ParseExpression();
        parser.Consume("then");

        var truePart = parser.ParseExpression();

        parser.Consume("else");

        var falsePart = parser.ParseExpression();

        return new IfNode(condition, truePart, falsePart);
    }
}