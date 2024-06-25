using System.Linq;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;
using System.Collections.Immutable;

namespace Furesoft.PrattParser;

//ToDo: rewrite with "with-syntax"
public abstract class Rewriter : IVisitor<AstNode>
{
    public virtual AstNode Rewrite<T>(LiteralNode<T> literal) => literal;
    public virtual AstNode Rewrite(BinaryOperatorNode binary)
    {
        var left = binary.LeftExpr.Accept(this);
        var right = binary.RightExpr.Accept(this);

        return new BinaryOperatorNode(left, binary.Operator, right);
    }

    public virtual AstNode Rewrite(CallNode call)
    {
        var args = call.Arguments.Select(arg => arg.Accept(this)).ToImmutableList();

        return new CallNode(call.FunctionExpr, args);
    }

    public AstNode Visit(AstNode node)
    {
        var rewrittenNode = node;

        if (node is LiteralNode<ulong> lLit)
        {
            rewrittenNode = Rewrite(lLit);
        }
        else if (node is LiteralNode<double> dLit)
        {
            rewrittenNode = Rewrite(dLit);
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

        return new BlockNode(block.SeperatorSymbol, block.Terminator)
            .WithChildren(children);
    }
}
