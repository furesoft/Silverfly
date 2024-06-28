using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;
using Sample.Nodes;

namespace Sample;

public class RewriterVisitor : Rewriter
{
    public override AstNode Rewrite(LiteralNode literal)
    {
        if (literal.Value is ulong value)
        {
            return literal with
            {
                Value = (double)value
            };
        }

        return literal;
    }

    public override AstNode Rewrite(CallNode call)
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

    public override AstNode RewriteOther(AstNode node)
    {
        if (node is LambdaNode l)
        {
            return l with
            {
                Value = Visit(l.Value)
            };
        }

        return node;
    }
}
