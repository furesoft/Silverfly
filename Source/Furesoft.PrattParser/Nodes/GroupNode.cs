namespace Furesoft.PrattParser.Nodes;

public class GroupNode(Symbol leftSymbol, Symbol rightSymbol, AstNode expr) : AstNode
{
    public Symbol LeftSymbol { get; } = leftSymbol;
    public Symbol RightSymbol { get; } = rightSymbol;
    public AstNode Expr { get; } = expr;
}
