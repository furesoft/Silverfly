using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Silverfly.Nodes;

/// <summary>
///     Represents a block node in an abstract syntax tree (AST), which contains a list of child nodes.
/// </summary>
public class BlockNode : AstNode
{
    public BlockNode(Symbol seperatorSymbol, Symbol terminator)
    {
        Properties.Set(nameof(SeperatorSymbol), seperatorSymbol);
        Properties.Set(nameof(Terminator), terminator);
    }

    public Symbol SeperatorSymbol
    {
        get => Properties.GetOrThrow<Symbol>(nameof(SeperatorSymbol));
    }

    public Symbol Terminator
    {
        get => Properties.GetOrThrow<Symbol>(nameof(Terminator));
    }

    /// <summary>
    ///     Sets the children nodes of this block node to the specified list of nodes.
    /// </summary>
    /// <param name="nodes">The list of child nodes to set.</param>
    /// <returns>This block node with the updated children nodes.</returns>
    public BlockNode WithChildren(ImmutableList<AstNode> nodes)
    {
        foreach (var child in nodes)
        {
            Children.Add(child);
        }

        return this;
    }

    /// <summary>
    ///     Get children of a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<T> GetChildren<T>()
    {
        return Children.OfType<T>();
    }

    /// <summary>
    ///     Checks if it has at least one child of a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasChild<T>()
    {
        return Children.Any(n => n is T);
    }
}
