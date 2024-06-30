namespace Silverfly.Nodes;

public record GroupNode(Symbol LeftSymbol, Symbol RightSymbol, AstNode Expr) : AstNode
{
}
