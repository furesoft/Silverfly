using System.Collections.Immutable;
using Silverfly.Generator;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func;

[Visitor]
public partial class RewriterVisitor : Rewriter
{
    protected override AstNode VisitUnknown(AstNode node) => node;

    private AstNode Rewrite(TupleBindingNode node)
    {
        return node with
        {
            Value = Visit(node.Value)
        };
    }

    private AstNode Rewrite(TupleNode node)
    {
        return node with
        {
            Values = node.Values.Select(Visit).ToImmutableList()
        };
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

        if (call.FunctionExpr is NameNode name && name.Token.Text.Span == "add".AsSpan())
        {
            // Start with the first argument
            var result = rewritten.Arguments[0];

            // Iterate over the rest of the arguments and create a BinaryOperatorNode for each addition
            for (int i = 1; i < rewritten.Arguments.Count; i++)
            {
                result = new BinaryOperatorNode(result, name.Token.Rewrite("+"), rewritten.Arguments[i]);
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
