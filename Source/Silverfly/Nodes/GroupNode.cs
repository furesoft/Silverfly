namespace Silverfly.Nodes;

/// <summary>
/// Represents a group node in an abstract syntax tree (AST), which represents an expression enclosed within grouping symbols.
/// </summary>
public record GroupNode(Symbol LeftSymbol, Symbol RightSymbol, AstNode Expr) : AstNode
{
}
