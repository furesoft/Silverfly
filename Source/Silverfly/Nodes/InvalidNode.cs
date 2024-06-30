namespace Silverfly.Nodes;

/// <summary>If parsing fails <see cref="InvalidNode" /> will be returned</summary>
public record InvalidNode(Token Token) : AstNode
{
}
