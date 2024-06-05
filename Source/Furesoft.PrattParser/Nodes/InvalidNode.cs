namespace Furesoft.PrattParser.Nodes;

/// <summary>If parsing fails <see cref="InvalidNode" /> will be returns</summary>
public class InvalidNode(Token token) : AstNode
{
    public Token Token { get; } = token;
}
