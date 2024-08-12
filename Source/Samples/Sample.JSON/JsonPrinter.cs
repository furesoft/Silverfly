using Sample.JSON.Nodes;
using Silverfly;
using Silverfly.Nodes;

namespace Sample.JSON;

class JsonPrinter : NodeVisitor
{
    public JsonPrinter()
    {
        For<JsonObject>(VisitObject);
        For<LiteralNode>(VsitLiteral);
        For<JsonArray>(VisitArr);
    }

    private void VisitArr(JsonArray obj)
    {
        Console.Write("[");
        
        foreach (var value in obj.Values)
        {
            value.Accept(this);
            Console.Write(",");
        }
        
        Console.Write("]");
    }

    private void VsitLiteral(LiteralNode lit)
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
