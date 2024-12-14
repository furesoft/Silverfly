namespace Silverfly.Nodes;

/// <summary>If parsing fails <see cref="InvalidNode" /> will be returned</summary>
public class InvalidNode(Token token) : AstNode
{
    public Token Token
    {
        get => Properties.GetOrAdd(nameof(Token), _ => token);
    }
}
