namespace Silverfly.Nodes;

/// <summary>A wrapper to wrap an <see cref="Expression"/> into a statement</summary>
public class ExpressionStatement : StatementNode
{
    public AstNode Expression => Children.First;

    public ExpressionStatement(AstNode expression)
    {
        Children.Add(expression);
    }
}
