namespace Furesoft.PrattParser.Nodes;

public record GroupNode(Symbol LeftSymbol, Symbol RightSymbol, AstNode Expr) : AstNode
{

}
