using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet : IPrefixParselet<AstNode>
{
    private readonly Symbol _rightSymbol;

    public GroupParselet(Symbol rightSymbol)
    {
        _rightSymbol = rightSymbol;
    }

    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        var expression = parser.Parse();
        parser.Consume(_rightSymbol);

        return expression;
    }
}
