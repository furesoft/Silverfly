using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Parselets;

namespace Silverfly.Sample.Rockstar.Parselets;

public class AssignmentParselet : IPrefixParselet
{
    // Put 123 into X
    // Let my balance be 1000000
    public AstNode Parse(Parser parser, Token token)
    {
        Symbol expectedToken = token.Type == "put" ? "into" : "be";

        var name = parser.ParseExpression();

        var operatorToken = parser.Consume(expectedToken);

        var value = parser.ParseExpression();

        return new BinaryOperatorNode(name, operatorToken.Rewrite("="), value).WithRange(token, parser.LookAhead(0));
    }
}
