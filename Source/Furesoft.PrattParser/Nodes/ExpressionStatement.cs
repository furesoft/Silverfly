namespace Furesoft.PrattParser.Nodes;

/// <summary>A wrapper to wrap an <see cref="Expression"/> into a statement</summary>
public record ExpressionStatement(AstNode Expression) : StatementNode
{
}
