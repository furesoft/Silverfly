﻿using MrKWatkins.Ast.Listening;
using Silverfly.Nodes;
using Silverfly.Sample.JSON.Nodes;

namespace Silverfly.Sample.JSON;

internal class JsonPrinter
{
    public static CompositeListener<object, AstNode> Listener = CompositeListener<object, AstNode>
        .Build()
        .With(new ArrayListener())
        .With(new LiteralListener())
        .With(new JsonObjectListener())
        .ToListener();

    private class ArrayListener : Listener<object, AstNode, JsonArray>
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

    private class LiteralListener : Listener<object, AstNode, LiteralNode>
    {
        protected override void ListenToNode(object context, LiteralNode node)
        {
            Console.Write(node.Value);
        }
    }

    private class JsonObjectListener : Listener<object, AstNode, JsonObject>
    {
        protected override void ListenToNode(object context, JsonObject node)
        {
            Console.WriteLine();
            foreach (var member in node.Members)
            {
                Console.WriteLine(member.Key + ":");

                Listen(null!, member.Value);
                Console.WriteLine();
            }
        }
    }
}
