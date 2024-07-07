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
    public virtual TReturn Visit(AstNode node, TTag tag)
    {
        if (!HasVisitor(node))
        {
            return VisitUnknown(node, tag);
        }

        return AfterVisit((TReturn)InvokeVisitor(node, tag));
    }

    public void For<TNode>(Func<TNode, TTag, TReturn> visitor)
        where TNode : AstNode
    {
        Visitors[typeof(TNode)] = visitor;
    }

    protected virtual TReturn VisitUnknown(AstNode node, TTag tag) => default;
    protected virtual TReturn AfterVisit(TReturn node) => node;
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

    public void For<TNode>(Action<TNode, TTag> visitor)
        where TNode : AstNode
    {
        Visitors[typeof(TNode)] = visitor;
    }
}
