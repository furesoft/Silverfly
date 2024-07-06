using System;
using System.Collections.Generic;
using Silverfly.Nodes;

namespace Silverfly;

public abstract class NodeVisitorBase
{
    protected readonly Dictionary<Type, Delegate> Visitors = [];

    protected object InvokeVisitor(AstNode node)
    {
        return Visitors[node.GetType()].DynamicInvoke(node);
    }

    protected object InvokeVisitor(AstNode node, object tag)
    {
        return Visitors[node.GetType()]?.DynamicInvoke(node, tag);
    }

    protected bool HasVisitor(AstNode node) => Visitors.ContainsKey(node.GetType());
}
