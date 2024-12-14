using MrKWatkins.Ast.Listening;
using Sample.Brainfuck.Nodes;
using Silverfly.Nodes;

namespace Sample.Brainfuck;

public class EvaluationContext
{
    internal char[] _cells = new char[100];
    internal int _pointer;
}

public class EvalListener
{
    public static CompositeListener<EvaluationContext, AstNode> Listener
    {
        get => CompositeListener<EvaluationContext, AstNode>
            .Build()
            .With(new PrintListener())
            .With(new ReadListener())
            .With(new DecrementListener())
            .With(new IncrementListener())
            .With(new IncrementCellListener())
            .With(new DecrementCellListener())
            .With(new BlockListener())
            .With(new LoopListener())
            .ToListener();
    }

    private class PrintListener : Listener<EvaluationContext, AstNode, PrintNode>
    {
        protected override void ListenToNode(EvaluationContext context, PrintNode node)
        {
            Console.Write(context._cells[context._pointer]);
        }
    }

    private class ReadListener : Listener<EvaluationContext, AstNode, PrintNode>
    {
        protected override void ListenToNode(EvaluationContext context, PrintNode node)
        {
            context._cells[context._pointer] = Console.ReadKey().KeyChar;
        }
    }

    private class DecrementListener : Listener<EvaluationContext, AstNode, DecrementNode>
    {
        protected override void ListenToNode(EvaluationContext context, DecrementNode node)
        {
            context._pointer--;
        }
    }

    private class IncrementListener : Listener<EvaluationContext, AstNode, DecrementNode>
    {
        protected override void ListenToNode(EvaluationContext context, DecrementNode node)
        {
            context._pointer++;
        }
    }

    private class IncrementCellListener : Listener<EvaluationContext, AstNode, DecrementNode>
    {
        protected override void ListenToNode(EvaluationContext context, DecrementNode node)
        {
            context._cells[context._pointer]++;
        }
    }

    private class DecrementCellListener : Listener<EvaluationContext, AstNode, DecrementNode>
    {
        protected override void ListenToNode(EvaluationContext context, DecrementNode node)
        {
            context._cells[context._pointer]--;
        }
    }

    private class BlockListener : Listener<EvaluationContext, AstNode, BlockNode>
    {
        protected override void ListenToNode(EvaluationContext context, BlockNode node)
        {
            foreach (var child in node.Children)
            {
                Listen(context, child);
            }
        }
    }

    private class LoopListener : Listener<EvaluationContext, AstNode, LoopNode>
    {
        protected override void ListenToNode(EvaluationContext context, LoopNode node)
        {
            while (context._cells[context._pointer] != '\0')
            {
                foreach (var child in node.Children)
                {
                    Listen(context, child);
                }
            }
        }
    }
}
