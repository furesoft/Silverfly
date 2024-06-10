namespace Furesoft.PrattParser.Nodes;

/// <summary>A wrapper to wrap an <see cref="Expression"/> into a statement</summary>
public class ExpressionStatement : StatementNode
{
    public AstNode Expression { get; private set; }

    public ExpressionStatement WithExpression(AstNode expression)
    {
        this.Expression = expression; ;
        return this;
    }
}
