using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Silverfly.Nodes;

/// <summary>
/// Represents a block node in an abstract syntax tree (AST), which contains a list of child nodes.
/// </summary>
public record BlockNode(Symbol SeperatorSymbol, Symbol Terminator) : AstNode
{
    /// <summary>
    /// Gets or sets the children nodes of this block node.
    /// </summary>
    public ImmutableList<AstNode> Children { get; set; }

    /// <summary>
    /// Sets the children nodes of this block node to the specified list of nodes.
    /// </summary>
    /// <param name="nodes">The list of child nodes to set.</param>
    /// <returns>This block node with the updated children nodes.</returns>
    public BlockNode WithChildren(ImmutableList<AstNode> nodes)
    {
        Children = nodes;
        return this;
    }

    /// <summary>
    /// Get children of a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<T> GetChildren<T>()
    {
        return Children.OfType<T>();
    }

    /// <summary>
    /// Checks if it has at least one child of a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasChild<T>()
    {
        return Children.Any(n => n is T);
    }
}
