using Sample.Brainfuck.Nodes;
using Silverfly;
using Silverfly.Generator;
using Silverfly.Nodes;

namespace Sample.Brainfuck;

[Visitor]
public partial class EvalVisitor : NodeVisitor
{
    private int pointer = 0;
    char[] cells = new char[100];

    [VisitorCondition("_.Token == '.'")]
    void Print(OperationNode node)
    {
        Console.WriteLine(cells[pointer]);
    }

    [VisitorCondition("_.Token == ','")]
    void Read(OperationNode node)
    {
        cells[pointer] = Console.ReadKey().KeyChar;
    }

    [VisitorCondition("_.Token == '<'")]
    void Decrement(OperationNode node)
    {
        pointer--;
    }

    [VisitorCondition("_.Token == '>'")]
    void Increment(OperationNode node)
    {
        pointer++;
    }

    [VisitorCondition("_.Token == '+'")]
    void IncrementCell(OperationNode node)
    {
        cells[pointer]++;
    }

    [VisitorCondition("_.Token == '-'")]
    void DecrementCell(OperationNode node)
    {
        cells[pointer]--;
    }

    [VisitorCondition("_.LeftSymbol == '['")]
    void Loop(GroupNode node)
    {
        while (cells[pointer] != 0) {
            Visit();
        }
    }
}

/*
>	ptr++;	inkrementiert den Zeiger
<	ptr--;	dekrementiert den Zeiger
+	cell[ptr]++;	inkrementiert den aktuellen Zellenwert
−	cell[ptr]--;	dekrementiert den aktuellen Zellenwert
.	putchar (cell[ptr]);	Gibt den aktuellen Zellenwert als ASCII-Zeichen auf der Standardausgabe aus
,	cell[ptr] = getchar();	Liest ein Zeichen von der Standardeingabe und speichert dessen ASCII-Wert in der aktuellen Zelle
[	while (cell[ptr]) {	Springt nach vorne, hinter den passenden ]-Befehl, wenn der aktuelle Zellenwert 0 ist
]	}	Springt zurück, hinter den passenden [-Befehl, wenn der aktuelle Zellenwert nicht 0 ist
*/
