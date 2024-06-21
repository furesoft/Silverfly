using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet(Symbol leftSymbol, Symbol rightSymbol, Symbol tag) : IPrefixParselet
{
    public Symbol LeftSymbol { get; } = leftSymbol;
    public Symbol RightSymbol { get; } = rightSymbol;
    public Symbol Tag { get; } = tag;

    public AstNode Parse(Parser parser, Token token)
    {
        var expression = parser.ParseExpression();
        var rightToken = parser.Consume(RightSymbol);

        var node = new GroupNode(LeftSymbol, RightSymbol, expression)
                .WithRange(token, rightToken);

        expression.WithParent(node);

        return node;
    }
}
