using Sample.Brainfuck.Nodes;
using Silverfly;
using Silverfly.Nodes;

namespace Sample.Brainfuck;

public class EvalVisitor : NodeVisitor
{
    private int _pointer;
    readonly char[] _cells = new char[100];

    public EvalVisitor()
    {
        For<OperationNode>(Print, _=> _.Token == ".");
        For<OperationNode>(Read, _=> _.Token == ",");
        For<OperationNode>(Decrement, _=> _.Token == "<");
        For<OperationNode>(Increment, _=> _.Token == ">");
        For<OperationNode>(DecrementCell, _=> _.Token == "-");
        For<OperationNode>(IncrementCell, _=> _.Token == "+");
        For<BlockNode>(Loop, _ => _.Tag == "loop");
        For<BlockNode>(Block, _ => _.Tag == null);
    }

    void Print(OperationNode node)
    {
        Console.Write(_cells[_pointer]);
    }

    void Read(OperationNode node)
    {
        _cells[_pointer] = Console.ReadKey().KeyChar;
    }

    void Decrement(OperationNode node)
    {
        _pointer--;
    }

    void Increment(OperationNode node)
    {
        _pointer++;
    }

    void IncrementCell(OperationNode node)
    {
        _cells[_pointer]++;
    }

    void DecrementCell(OperationNode node)
    {
        _cells[_pointer]--;
    }

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

    void Block(BlockNode node)
    {
        foreach (var child in node.Children)
        {
            Visit(child);
        }
    }
}
