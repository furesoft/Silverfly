namespace Silverfly.Nodes;

/// <summary>A wrapper to wrap an <see cref="Expression" /> into a statement</summary>
public class ExpressionStatement : StatementNode
{
    public ExpressionStatement(AstNode expression)
    {
        Children.Add(expression);
    }

    public AstNode Expression
    {
        get => Children.First;
    }
}
