using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet(Symbol rightSymbol) : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var expression = parser.ParseExpression();
        var rightToken = parser.Consume(rightSymbol);

        return expression.WithRange(token, rightToken);
    }
}
