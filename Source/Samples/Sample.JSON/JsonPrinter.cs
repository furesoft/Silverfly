using Silverfly.Nodes;
using Silverfly.Sample.JSON.Nodes;

namespace Silverfly.Sample.JSON;

public class JsonPrinter : NodeVisitor
{
    public JsonPrinter()
    {
        For<JsonArray>(VisitArray, _ => _.Values.Count > 0);
        For<LiteralNode>(VisitLiteral);
        For<JsonObject>(VisitObject);
    }

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

    void Indent(string src)
    {
        _indentLevel++;
        Console.Write(new string(' ', _indentLevel*2));
        Console.Write(src);
    }
}
