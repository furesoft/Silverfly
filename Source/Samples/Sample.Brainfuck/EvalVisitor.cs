using Sample.Brainfuck.Nodes;
using Silverfly;
using Silverfly.Generator;
using Silverfly.Nodes;

namespace Sample.Brainfuck;

[Visitor]
public partial class EvalVisitor : NodeVisitor
{
    private int _pointer = 0;
    readonly char[] _cells = new char[100];

    [VisitorCondition("_.Token == '.'")]
    void Print(OperationNode node)
    {
        Console.WriteLine(_cells[_pointer]);
    }

    [VisitorCondition("_.Token == ','")]
    void Read(OperationNode node)
    {
        _cells[_pointer] = Console.ReadKey().KeyChar;
    }

    [VisitorCondition("_.Token == '<'")]
    void Decrement(OperationNode node)
    {
        _pointer--;
    }

    [VisitorCondition("_.Token == '>'")]
    void Increment(OperationNode node)
    {
        _pointer++;
    }

    [VisitorCondition("_.Token == '+'")]
    void IncrementCell(OperationNode node)
    {
        _cells[_pointer]++;
    }

    [VisitorCondition("_.Token == '-'")]
    void DecrementCell(OperationNode node)
    {
        _cells[_pointer]--;
    }

    [VisitorCondition("_.Tag == 'loop'")]
    void Loop(BlockNode node)
    {
        while (_cells[_pointer] != '\0')
        {
            foreach (var child in node.Children)
            {
                Visit(child);
            }
        }
    }

    [VisitorCondition("_.Tag == null")]
    void Block(BlockNode node)
    {
        foreach (var child in node.Children)
        {
            Visit(child);
        }
    }
}
