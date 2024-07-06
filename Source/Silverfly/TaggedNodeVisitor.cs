using System;
using Silverfly.Nodes;

namespace Silverfly;

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
