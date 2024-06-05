namespace Furesoft.PrattParser.Nodes;

/// <summary>A wrapper to wrap an <see cref="Expression"/> into a statement</summary>
public class ExpressionStatement(AstNode expression) : StatementNode
{
    public AstNode Expression { get; } = expression;
}
