using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet(Symbol leftSymbol, Symbol rightSymbol) : IPrefixParselet
{
    public Symbol LeftSymbol { get; } = leftSymbol;
    public Symbol RightSymbol { get; } = rightSymbol;

    public AstNode Parse(Parser parser, Token token)
    {
        var expression = parser.ParseExpression();
        var rightToken = parser.Consume(RightSymbol);

        return new GroupNode(leftSymbol, rightSymbol, expression)
                .WithRange(token, rightToken);
    }
}
