#nullable enable
using Silverfly.Nodes;

namespace Silverfly.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet(Symbol leftSymbol, Symbol rightSymbol, object? tag) : IPrefixParselet, ISymbolDiscovery
{
    public Symbol LeftSymbol { get; } = leftSymbol;
    public Symbol RightSymbol { get; } = rightSymbol;
    public object? Tag { get; } = tag;

    public AstNode Parse(Parser parser, Token token)
    {
        var expression = parser.ParseExpression();
        var rightToken = parser.Consume(RightSymbol);

        var node = new GroupNode(LeftSymbol, RightSymbol, expression)
                .WithTag(tag)
                .WithRange(token, rightToken);

        expression.WithParent(node);

        return node;
    }

    public Symbol[] GetSymbols()
    {
        return [LeftSymbol, RightSymbol];
    }
}
