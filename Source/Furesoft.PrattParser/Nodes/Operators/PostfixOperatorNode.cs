namespace Furesoft.PrattParser.Nodes.Operators;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixOperatorNode(AstNode left, Symbol op) : AstNode
{
    public AstNode Expr { get; } = left;
    public Symbol Operator { get; } = op;
}
