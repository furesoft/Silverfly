using System.Linq;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;
using System.Collections.Immutable;

namespace Furesoft.PrattParser;

public abstract class Rewriter : IVisitor<AstNode>
{
    public virtual AstNode Rewrite(LiteralNode literal) => literal;
    public virtual AstNode Rewrite(BinaryOperatorNode binary)
    {
        var left = binary.LeftExpr.Accept(this);
        var right = binary.RightExpr.Accept(this);

        return binary with {
            LeftExpr = left,
            RightExpr = right
        };
    }

    public virtual AstNode Rewrite(CallNode call)
    {
        var args = call.Arguments.Select(arg => arg.Accept(this)).ToImmutableList();

        return call with {
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
        else if (node is BlockNode block)
        {
            rewrittenNode = Rewrite(block);
        }

        return rewrittenNode
            .WithRange(node.Range)
            .WithParent(node.Parent);
    }

    private AstNode Rewrite(BlockNode block)
    {
        var children = block.Children.Select(Visit).ToImmutableList();

        return block with {
            Children = children
        };
    }
}
