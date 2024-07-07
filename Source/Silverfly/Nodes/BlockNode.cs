using System.Collections.Immutable;

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
}
