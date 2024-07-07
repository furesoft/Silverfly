namespace Silverfly.Nodes.Operators;

/// <summary>
/// Represents a ternary operator node in an abstract syntax tree (AST).
/// </summary>
public record TernaryOperatorNode(AstNode FirstExpr, AstNode SecondExpr, AstNode ThirdExpr) : AstNode
{
}
