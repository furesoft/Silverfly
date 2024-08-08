namespace Silverfly.Nodes;

/// <summary>
/// Represents a literal node in an abstract syntax tree (AST), which holds a literal value.
/// </summary>
public record LiteralNode(object Value, Token Token) : AstNode
{
}
