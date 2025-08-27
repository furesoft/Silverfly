namespace Silverfly.Nodes;

/// <summary>
///     Represents a literal node in an abstract syntax tree (AST), which holds a literal value.
/// </summary>
public class LiteralNode(object value, Token token) : ExpressionNode
{
    public object Value
    {
        get => Properties.GetOrAdd(nameof(Value), _ => value);
    }

    public Token Token
    {
        get => Properties.GetOrAdd(nameof(Token), _ => token);
    }
}
