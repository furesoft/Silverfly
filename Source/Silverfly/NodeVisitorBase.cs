using System;
using System.Collections.Generic;
using Silverfly.Nodes;

namespace Silverfly;

public abstract class NodeVisitorBase
{
    protected readonly Dictionary<Type, List<(Delegate Visitor, Predicate<AstNode> Predicate)>> Visitors = new();

    protected void InvokeVisitor(AstNode node)
    {
        if (Visitors.TryGetValue(node.GetType(), out var visitorList))
        {
            foreach (var (visitor, predicate) in visitorList)
            {
                if (predicate(node))
                {
                    visitor.DynamicInvoke(node);
                }
            }
        }
    }

    protected TReturn InvokeVisitor<TReturn>(AstNode node)
    {
        if (Visitors.TryGetValue(node.GetType(), out var visitorList))
        {
            foreach (var (visitor, predicate) in visitorList)
            {
                if (predicate(node))
                {
                    return (TReturn)visitor.DynamicInvoke(node);
                }
            }
        }

        return default;
    }

    protected void InvokeVisitor(AstNode node, object tag)
    {
        if (Visitors.TryGetValue(node.GetType(), out var visitorList))
        {
            foreach (var (visitor, predicate) in visitorList)
            {
                if (predicate(node))
                {
                    visitor.DynamicInvoke(node, tag);
                }
            }
        }
    }

    protected TReturn InvokeVisitor<TReturn>(AstNode node, object tag)
    {
        if (Visitors.TryGetValue(node.GetType(), out var visitorList))
        {
            foreach (var (visitor, predicate) in visitorList)
            {
                if (predicate(node))
                {
                    return (TReturn)visitor.DynamicInvoke(node, tag);
                }
            }
        }

        return default;
    }

    protected bool HasVisitor(AstNode node) => Visitors.ContainsKey(node.GetType());

    protected void RegisterVisitor<T>(Delegate visitor, Predicate<AstNode> predicate) where T : AstNode
    {
        if (!Visitors.ContainsKey(typeof(T)))
        {
            Visitors[typeof(T)] = new List<(Delegate, Predicate<AstNode>)>();
        }

        Visitors[typeof(T)].Add((visitor, predicate));
    }
}
