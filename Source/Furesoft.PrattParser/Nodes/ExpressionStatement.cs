namespace Furesoft.PrattParser.Nodes;

public class ExpressionStatement(AstNode expression) : StatementNode
{
    public AstNode Expression { get; } = expression;
}
