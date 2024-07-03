using System.Linq;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using System.Collections.Immutable;

namespace Silverfly;

public abstract class Rewriter : NodeVisitor<AstNode>
{
    public Rewriter()
    {
        For<LiteralNode>(Rewrite);
        For<NameNode>(Rewrite);
        For<GroupNode>(Rewrite);
        For<BinaryOperatorNode>(Rewrite);
        For<CallNode>(Rewrite);
        For<PrefixOperatorNode>(Rewrite);
        For<PostfixOperatorNode>(Rewrite);
        For<BinaryOperatorNode>(Rewrite);
        For<BlockNode>(Rewrite);
    }

    protected AstNode Rewrite(LiteralNode literal) => literal;
    protected AstNode Rewrite(NameNode literal) => literal;

    protected AstNode Rewrite(GroupNode group) => Visit(group.Expr);

    protected AstNode Rewrite(BinaryOperatorNode binary)
    {
        var left = binary.LeftExpr.Accept(this);
        var right = binary.RightExpr.Accept(this);

        return binary with
        {
            LeftExpr = left,
            RightExpr = right
        };
    }

    protected AstNode Rewrite(CallNode call)
    {
        var args = call.Arguments.Select(arg => arg.Accept(this)).ToImmutableList();

        return call with
        {
            FunctionExpr = Visit(call.FunctionExpr),
            Arguments = args
        };
    }

    protected override AstNode AfterVisit(AstNode node)
    {
        return node with
        {
            Range = node.Range,
            Parent = node.Parent
        };
    }

    protected AstNode Rewrite(PostfixOperatorNode post)
    {
        return post with
        {
            Expr = Visit(post.Expr)
        };
    }

    protected AstNode Rewrite(PrefixOperatorNode pre)
    {
        return pre with
        {
            Expr = Visit(pre.Expr)
        };
    }

    protected AstNode Rewrite(BlockNode block)
    {
        var children = block.Children.Select(Visit).ToImmutableList();

        return block with
        {
            Children = children
        };
    }
}
