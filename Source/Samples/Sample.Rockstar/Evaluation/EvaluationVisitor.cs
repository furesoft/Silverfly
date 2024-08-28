using Silverfly.Generator;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Text;

namespace Silverfly.Sample.Rockstar.Evaluation;

[Visitor]
public partial class EvaluationVisitor : TaggedNodeVisitor<object, Scope>
{
    [VisitorCondition("_.Operator == '='")]
    private object VisitAssignment(BinaryOperatorNode node, Scope scope)
    {
        if (node.LeftExpr is not NameNode name) return null;

        scope.Define(name.Token.Text.Trim().ToString(), Visit(node.RightExpr, scope));

        return null;
    }

    private object VisitCall(CallNode call, Scope scope)
    {
        if (call.FunctionExpr is NameNode name && name.Token == "print")
        {
            var arg = Visit(call.Arguments[0], scope);

            if (arg is null && call.Arguments[0] is NameNode n)
            {
                call.Arguments[0].AddMessage(MessageSeverity.Error, $"Variable '{n.Token.Text}' is not defined");
                return null;
            }
            
            Console.WriteLine(arg);
        }

        return null;
    }

    private object VisitLiteral(LiteralNode node, Scope scope)
    {
        return node.Value;
    }

    private object VisitName(NameNode node, Scope scope)
    {
        return scope.Get(node.Token.Text.ToString());
    }

    private object VisitBlock(BlockNode block, Scope scope)
    {
        foreach (var child in block.Children)
        {
            Visit(child, scope);
        }

        return null;
    }
}
