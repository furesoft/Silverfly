using System;
using Silverfly.Nodes;

namespace Silverfly;

/// <summary>
/// Represents a base class for a visitor that processes nodes with a specific tag and returns a result.
/// </summary>
/// <typeparam name="TReturn">The type of the result returned by the visitor.</typeparam>
/// <typeparam name="TTag">The type of the tag associated with nodes visited by the visitor.</typeparam>
public abstract class TaggedNodeVisitor<TReturn, TTag> : NodeVisitorBase
{
    /// <summary>
    /// Visits the specified AST node using the appropriate visitor method, with an additional tag parameter.
    /// </summary>
    /// <param name="node">The AST node to visit.</param>
    /// <param name="tag">An additional tag parameter that can be used by the visitor methods.</param>
    /// <returns>The result of visiting the node, which is of type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method checks if a specific visitor method is registered for the given <paramref name="node"/> using the <see cref="HasVisitor"/> method.
    /// If a registered visitor is found, it is invoked with the <paramref name="tag"/> parameter. If no specific visitor is found, the <see cref="VisitUnknown"/> method is called.
    /// After visiting the node, the result is processed through the <see cref="AfterVisit"/> method before being returned.
    /// </remarks>
    public virtual TReturn Visit(AstNode node, TTag tag)
    {
        if (!HasVisitor(node))
        {
            return VisitUnknown(node, tag);
        }

        return AfterVisit(node, (TReturn)InvokeVisitor(node, tag));
    }

    /// <summary>
    /// Registers a visitor function for nodes of type <typeparamref name="TNode"/> with an additional tag parameter.
    /// </summary>
    /// <typeparam name="TNode">The type of AST node that the visitor function will handle. Must derive from <see cref="AstNode"/>.</typeparam>
    /// <param name="visitor">The function that will be called to visit nodes of type <typeparamref name="TNode"/> with an additional <paramref name="tag"/> parameter.</param>
    /// <remarks>
    /// This method stores the provided <paramref name="visitor"/> function in a dictionary, associating it with the type <typeparamref name="TNode"/>.
    /// The function takes an additional <paramref name="tag"/> parameter that can be used for custom processing of nodes.
    /// </remarks>
    public void For<TNode>(Func<TNode, TTag, TReturn> visitor)
        where TNode : AstNode
    {
        Visitors[typeof(TNode)] = visitor;
    }

    /// <summary>
    /// Provides a default implementation for visiting nodes that do not have a specific visitor registered, with an additional tag parameter.
    /// </summary>
    /// <param name="node">The AST node that does not have a specific visitor.</param>
    /// <param name="tag">An additional tag parameter that can be used in the default processing.</param>
    /// <returns>The default value for the return type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method is called when there is no specific visitor function registered for the given <paramref name="node"/> type.
    /// The default implementation simply returns the default value for the <typeparamref name="TReturn"/> type.
    /// This method can be overridden in derived classes to provide custom handling for unknown nodes using the <paramref name="tag"/> parameter.
    /// </remarks>
    protected virtual TReturn VisitUnknown(AstNode node, TTag tag) => default;
    
    /// <summary>
    /// Processes the result after visiting a node, with an additional tag parameter.
    /// </summary>
    /// <param name="node">The AST node that was visited.</param>
    /// <param name="value">The result of visiting the node, of type <typeparamref name="TReturn"/>.</param>
    /// <returns>The processed result, which is of type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method is called after the visitor function has processed a node. The default implementation simply returns the input <paramref name="value"/> unchanged.
    /// This method can be overridden in derived classes to perform additional processing or transformation on the result of the visit.
    /// </remarks>
    protected virtual TReturn AfterVisit(AstNode node, TReturn value) => value;
}

public abstract class TaggedNodeVisitor<TTag> : NodeVisitorBase
{
    public void Visit(AstNode node, TTag tag)
    {
        if (!HasVisitor(node))
        {
            VisitUnknown(node, tag);
        }

        InvokeVisitor(node, tag);

        AfterVisit(node);
    }
    
    public void For<TNode>(Action<TNode, TTag> visitor)
        where TNode : AstNode
    {
        Visitors[typeof(TNode)] = visitor;
    }

    public virtual void Visit(BlockNode block, TTag tag)
    {
        foreach (var child in block.Children)
        {
            Visit(child, tag);
        }
    }

    protected virtual void VisitUnknown(AstNode node, TTag tag)
    {
    }

    protected virtual void AfterVisit(AstNode node)
    {
    }
}
