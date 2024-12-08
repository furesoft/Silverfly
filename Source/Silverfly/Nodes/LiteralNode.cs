namespace Silverfly.Nodes;

/// <summary>
/// Represents a literal node in an abstract syntax tree (AST), which holds a literal value.
/// </summary>
public class LiteralNode(object value, Token token) : AstNode
{
    public object Value => Properties.GetOrAdd<object>(nameof(Value), _ => value);
    public Token Token => Properties.GetOrAdd<Token>(nameof(Token), _ => token);
}
