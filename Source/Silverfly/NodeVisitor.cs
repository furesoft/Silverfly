using System;
using Silverfly.Nodes;

namespace Silverfly;

/// <summary>
/// Represents a base class for visiting and processing abstract syntax tree (AST) nodes.
/// </summary>
/// <typeparam name="TReturn">The type of the return value when visiting an AST node.</typeparam>
public abstract class NodeVisitor<TReturn> : NodeVisitorBase
{
    /// <summary>
    /// Visits the specified AST (Abstract Syntax Tree) node using the appropriate visitor method. If no specific visitor is registered for the node, falls back to the default handling.
    /// </summary>
    /// <param name="node">The AST node to visit.</param>
    /// <returns>The result of visiting the node, which is of type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method first checks if a visitor method has been registered for the given <paramref name="node"/> using the <see cref="NodeVisitorBase.HasVisitor"/> method.
    /// If a registered visitor is found, it invokes that visitor and returns the result. If no specific visitor is found, it invokes the <see cref="VisitUnknown"/> method.
    /// After the visit, it processes the result through the <see cref="AfterVisit"/> method.
    /// </remarks>
    public virtual TReturn Visit(AstNode node)
    {
        if (!HasVisitor(node))
        {
            return VisitUnknown(node);
        }

        return AfterVisit((TReturn)InvokeVisitor(node));
    }

    /// <summary>
    /// Registers a visitor function for nodes of type <typeparamref name="TNode"/>.
    /// </summary>
    /// <typeparam name="TNode">The type of AST node that the visitor function will handle. Must derive from <see cref="AstNode"/>.</typeparam>
    /// <param name="visitor">The function that will be called to visit nodes of type <typeparamref name="TNode"/>.</param>
    /// <remarks>
    /// This method stores the provided <paramref name="visitor"/> function in a dictionary, associating it with the type <typeparamref name="TNode"/>.
    /// When visiting nodes of this type, the stored function will be used to process them.
    /// </remarks>
    public void For<TNode>(Func<TNode, TReturn> visitor)
        where TNode : AstNode
    {
        Visitors[typeof(TNode)] = visitor;
    }

    /// <summary>
    /// Provides a default implementation for visiting nodes that do not have a specific visitor registered.
    /// </summary>
    /// <param name="node">The AST (Abstract Syntax Tree) node that does not have a specific visitor.</param>
    /// <returns>The default value for the return type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method is called when there is no specific visitor function registered for the given <paramref name="node"/> type.
    /// The default implementation simply returns the default value for the <typeparamref name="TReturn"/> type.
    /// This method can be overridden in derived classes to provide custom handling for unknown nodes.
    /// </remarks>
    protected virtual TReturn VisitUnknown(AstNode node) => default;
    
    /// <summary>
    /// Processes the result after visiting a node.
    /// </summary>
    /// <param name="node">The result of visiting the node, of type <typeparamref name="TReturn"/>.</param>
    /// <returns>The processed result, which is the same as the input in the default implementation.</returns>
    /// <remarks>
    /// This method is called after the visitor function has processed a node. The default implementation simply returns the input <paramref name="node"/> unchanged.
    /// This method can be overridden in derived classes to perform additional processing or transformation on the result of the visit.
    /// </remarks>
    protected virtual TReturn AfterVisit(TReturn node) => node;
}

public abstract class NodeVisitor : NodeVisitorBase
{
    /// <summary>
    /// Visits the specified AST (Abstract Syntax Tree) node using the appropriate visitor method. If no specific visitor is registered for the node, falls back to the default handling.
    /// </summary>
    /// <param name="node">The AST node to visit.</param>
    /// <returns>The result of visiting the node, which is of type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method first checks if a visitor method has been registered for the given <paramref name="node"/> using the <see cref="NodeVisitorBase.HasVisitor"/> method.
    /// If a registered visitor is found, it invokes that visitor and returns the result. If no specific visitor is found, it invokes the <see cref="VisitUnknown"/> method.
    /// After the visit, it processes the result through the <see cref="AfterVisit"/> method.
    /// </remarks>
    public void Visit(AstNode node)
    {
        if (!HasVisitor(node))
        {
            VisitUnknown(node);
        }

        InvokeVisitor(node);

        AfterVisit(node);
    }

    /// <summary>
    /// Registers a visitor function for nodes of type <typeparamref name="TNode"/>.
    /// </summary>
    /// <typeparam name="TNode">The type of AST node that the visitor function will handle. Must derive from <see cref="AstNode"/>.</typeparam>
    /// <param name="visitor">The function that will be called to visit nodes of type <typeparamref name="TNode"/>.</param>
    /// <remarks>
    /// This method stores the provided <paramref name="visitor"/> function in a dictionary, associating it with the type <typeparamref name="TNode"/>.
    /// When visiting nodes of this type, the stored function will be used to process them.
    /// </remarks>
    public void For<TNode>(Action<TNode> visitor)
        where TNode : AstNode
    {
        Visitors[typeof(TNode)] = visitor;
    }
    
    /// <summary>
    /// Visits a <see cref="BlockNode"/> and recursively visits its child nodes.
    /// </summary>
    /// <param name="block">The <see cref="BlockNode"/> to visit.</param>
    /// <remarks>
    /// This method iterates through each child node of the specified <paramref name="block"/> and calls the <see cref="Visit"/> method on each child.
    /// It ensures that all child nodes within the block are visited in turn.
    /// This method can be overridden in derived classes to provide custom processing for <see cref="BlockNode"/> instances.
    /// </remarks>
    public virtual void Visit(BlockNode block)
    {
        foreach (var child in block.Children)
        {
            Visit(child);
        }
    }

    /// <summary>
    /// Provides a default implementation for visiting nodes that do not have a specific visitor registered.
    /// </summary>
    /// <param name="node">The AST (Abstract Syntax Tree) node that does not have a specific visitor.</param>
    /// <returns>The default value for the return type <typeparamref name="TReturn"/>.</returns>
    /// <remarks>
    /// This method is called when there is no specific visitor function registered for the given <paramref name="node"/> type.
    /// The default implementation simply returns the default value for the <typeparamref name="TReturn"/> type.
    /// This method can be overridden in derived classes to provide custom handling for unknown nodes.
    /// </remarks>
    protected virtual void VisitUnknown(AstNode node)
    {
    }

    /// <summary>
    /// Processes the result after visiting a node.
    /// </summary>
    /// <param name="node">The result of visiting the node, of type <typeparamref name="TReturn"/>.</param>
    /// <returns>The processed result, which is the same as the input in the default implementation.</returns>
    /// <remarks>
    /// This method is called after the visitor function has processed a node. The default implementation simply returns the input <paramref name="node"/> unchanged.
    /// This method can be overridden in derived classes to perform additional processing or transformation on the result of the visit.
    /// </remarks>
    protected virtual void AfterVisit(AstNode node)
    {
    }
}
