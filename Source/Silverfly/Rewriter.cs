using System.Linq;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using System.Collections.Immutable;
using System;

namespace Silverfly;

public abstract class Rewriter : IVisitor<AstNode>
{
    public virtual AstNode Rewrite(LiteralNode literal) => literal;

    public virtual AstNode RewriteOther(AstNode node) => node;

    public virtual AstNode Rewrite(GroupNode group) => Visit(group.Expr);

    public virtual AstNode Rewrite(BinaryOperatorNode binary)
    {
        var left = binary.LeftExpr.Accept(this);
        var right = binary.RightExpr.Accept(this);

        return binary with
        {
            LeftExpr = left,
            RightExpr = right
        };
    }

    public virtual AstNode Rewrite(CallNode call)
    {
        var args = call.Arguments.Select(arg => arg.Accept(this)).ToImmutableList();

        return call with
        {
            FunctionExpr = Visit(call.FunctionExpr),
            Arguments = args
        };
    }

    public AstNode Visit(AstNode node)
    {
        var rewrittenNode = node;

        if (node is LiteralNode lit)
        {
            rewrittenNode = Rewrite(lit);
        }
        else if (node is CallNode call)
        {
            rewrittenNode = Rewrite(call);
        }
        else if (node is BinaryOperatorNode bin)
        {
            rewrittenNode = Rewrite(bin);
        }
        else if (node is PrefixOperatorNode pre)
        {
            rewrittenNode = Rewrite(pre);
        }
        else if (node is PostfixOperatorNode post)
        {
            rewrittenNode = Rewrite(post);
        }
        else if (node is BlockNode block)
        {
            rewrittenNode = Rewrite(block);
        }
        else if (node is GroupNode group)
        {
            rewrittenNode = Rewrite(group);
        }
        else
        {
            rewrittenNode = RewriteOther(node);
        }

        return rewrittenNode with
        {
            Range = node.Range,
            Parent = node.Parent
        };
    }

    public virtual AstNode Rewrite(PostfixOperatorNode post)
    {
        return post with
        {
            Expr = Visit(post.Expr)
        };
    }

    public virtual AstNode Rewrite(PrefixOperatorNode pre)
    {
        return pre with
        {
            Expr = Visit(pre.Expr)
        };
    }

    public virtual AstNode Rewrite(BlockNode block)
    {
        var children = block.Children.Select(Visit).ToImmutableList();

        return block with
        {
            Children = children
        };
    }
}
