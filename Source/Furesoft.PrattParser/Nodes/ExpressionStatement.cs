namespace Furesoft.PrattParser.Nodes;

public class ExpressionStatement : StatementNode
{
    public AstNode Expression { get; }

    public ExpressionStatement(AstNode expression)
    {
        Expression = expression;
    }
}
