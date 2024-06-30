using System.Collections.Immutable;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Sample.Nodes;

namespace Sample;

public class RewriterVisitor : Rewriter
{
    public RewriterVisitor()
    {
        For<LiteralNode>(Rewrite);
        For<CallNode>(Rewrite);
        For<LambdaNode>(Rewrite);
        For<VariableBindingNode>(Rewrite);
    }
    private new AstNode Rewrite(LiteralNode literal)
    {
        if (literal.Value is ulong value)
        {
            return literal with
            {
                Value = (double)value
            };
        }
        else if (literal.Value is ImmutableList<AstNode> values)
        {
            return literal with
            {
                Value = values.Select(Visit).ToImmutableList()
            };
        }

        return literal;
    }

    private new AstNode Rewrite(CallNode call)
    {
        var rewritten = (CallNode)base.Rewrite(call);

        if (call.FunctionExpr is NameNode n && n.Name == "add")
        {
            // Start with the first argument
            var result = rewritten.Arguments[0];

            // Iterate over the rest of the arguments and create a BinaryOperatorNode for each addition
            for (int i = 1; i < rewritten.Arguments.Count; i++)
            {
                result = new BinaryOperatorNode(result, "+", rewritten.Arguments[i]);
            }

            return result;
        }

        return rewritten;
    }

    private AstNode Rewrite(LambdaNode node)
    {
        return node with
        {
            Value = Visit(node.Value)
        };
    }

    private AstNode Rewrite(VariableBindingNode node)
    {
        return node with
        {
            Value = Visit(node.Value)
        };
    }
}
