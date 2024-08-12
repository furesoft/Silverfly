using Sample.JSON.Nodes;
using Silverfly;
using Silverfly.Generator;
using Silverfly.Nodes;

namespace Sample.JSON;

[Visitor]
partial class JsonPrinter : NodeVisitor
{
    [VisitorCondition("_.Values.Count > 0")]
    private void VisitArray(JsonArray obj)
    {
        Console.Write("[");
        
        foreach (var value in obj.Values)
        {
            value.Accept(this);
            Console.Write(",");
        }
        
        Console.Write("]");
    }

    private void VisitLiteral(LiteralNode lit)
    {
        Console.Write(lit.Value);
    }

    private void VisitObject(JsonObject obj)
    {
        Console.WriteLine();
        foreach (var member in obj.Members)
        {
            Indent(member.Key + ":");
            
            member.Value.Accept(this);
            Console.WriteLine();
        }
    }

    private int _indentLevel = 0;
    
    [VisitorIgnore]
    void Indent(string src)
    {
        _indentLevel++;
        Console.Write(new string(' ', _indentLevel*2));
        Console.Write(src);
    }
}
