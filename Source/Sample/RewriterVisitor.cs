using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Sample;

public class RewriterVisitor : Rewriter
{
    public override AstNode Rewrite(LiteralNode literal)
    {
        if (literal.Value is ulong value)
        {
            return literal with {
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
            return new BinaryOperatorNode(rewritten.Arguments[0], "+", rewritten.Arguments[1]);
        }

        return rewritten;
    }

}
