using MrKWatkins.Ast.Listening;
using Sample.JSON.Nodes;
using Silverfly.Nodes;

namespace Sample.JSON;

class JsonPrinter
{
    public static CompositeListener<object, AstNode> Listener = CompositeListener<object, AstNode>
                        .Build()
                        .With(new ArrayListener())
                        .With(new LiteralListener())
                        .With(new JsonObjectListener())
                        .ToListener();
    private int _indentLevel = 0;

    class ArrayListener : Listener<object, AstNode, JsonArray>
    {
        protected override void ListenToNode(object context, JsonArray node)
        {
            Console.Write("[");

            foreach (var value in node.Children)
            {
                Listen(null!, value);
                Console.Write(",");
            }

            Console.Write("]");
        }
    }

    class LiteralListener : Listener<object, AstNode, LiteralNode>
    {
        protected override void ListenToNode(object context, LiteralNode node)
        {
            Console.Write(node.Value);
        }
    }

    class JsonObjectListener : Listener<object, AstNode, JsonObject>
    {
        protected override void ListenToNode(object context, JsonObject node)
        {
            Console.WriteLine();
            foreach (var member in node.Members)
            {
                Indent(member.Key + ":");

                Listen(null!, member.Value);
                Console.WriteLine();
            }
        }
    }

    void Indent(string src)
    {
        _indentLevel++;
        Console.Write(new string(' ', _indentLevel * 2));
        Console.Write(src);
    }
}
