namespace Silverfly.Nodes;

/// <summary>If parsing fails <see cref="InvalidNode" /> will be returned</summary>
public class InvalidNode(Token token) : AstNode
{
    public Token Token => Properties.GetOrAdd<Token>(nameof(Token), _ => token);
}
