using MrKWatkins.Ast.Listening;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Sample.Rockstar.Evaluation;

public class EvaluationListener
{
    public static CompositeListener<EvaluationContext, AstNode> Listener = CompositeListener<EvaluationContext, AstNode>
            .Build()
            .With(new BinaryListener())
            .With(new LiteralListener())
            .With(new NameListener())
            .With(new BlockListener())
            .With(new CallListener())
            .ToListener();

    class BinaryListener : Listener<EvaluationContext, AstNode, BinaryOperatorNode>
    {
        protected override void ListenToNode(EvaluationContext context, BinaryOperatorNode node)
        {
            if (node.Operator == "=")
            {
                if (node.Left is not NameNode name) return;
                Listen(context, node.Right);

                context.Scope.Define(name.Token.Text.Trim().ToString(), context.Stack.Pop());
            }
        }
    }

    class LiteralListener : Listener<EvaluationContext, AstNode, LiteralNode>
    {
        protected override void ListenToNode(EvaluationContext context, LiteralNode node)
        {
            context.Stack.Push(node.Value);
        }
    }

    class NameListener : Listener<EvaluationContext, AstNode, NameNode>
    {
        protected override void ListenToNode(EvaluationContext context, NameNode node)
        {
            context.Stack.Push(context.Scope.Get(node.Token.Text.ToString()));
        }
    }

    class BlockListener : Listener<EvaluationContext, AstNode, BlockNode>
    {
        protected override void ListenToNode(EvaluationContext context, BlockNode node)
        {
            foreach (var child in node.Children)
            {
                Listen(context, child);
            }
        }
    }

    class CallListener : Listener<EvaluationContext, AstNode, CallNode>
    {
        protected override void ListenToNode(EvaluationContext context, CallNode node)
        {
            if (node.FunctionExpr is NameNode name && name.Token == "print")
            {
                Listen(context, node.Arguments.First());

                var arg = context.Stack.Pop();

                if (arg is null && node.Arguments.First() is NameNode n)
                {
                    node.Arguments.First().AddError($"Variable '{n.Token.Text}' is not defined");
                    return;
                }

                Console.WriteLine(arg);
            }
        }
    }
}
