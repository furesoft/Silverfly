namespace Silverfly.Nodes;

/// <summary>
/// Represents a group node in an abstract syntax tree (AST), which represents an expression enclosed within grouping symbols.
/// </summary>
public class GroupNode : AstNode
{
    public GroupNode(Symbol leftSymbol, Symbol rightSymbol, AstNode expr)
    {
        Properties.Set(nameof(LeftSymbol), leftSymbol);
        Properties.Set(nameof(RightSymbol), rightSymbol);

        Children.Add(expr);
    }

    public Symbol LeftSymbol => Properties.GetOrThrow<Symbol>(nameof(LeftSymbol));
    public Symbol RightSymbol => Properties.GetOrThrow<Symbol>(nameof(RightSymbol));

    public AstNode Expr => Children.First;
}
