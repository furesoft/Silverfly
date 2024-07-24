using System;
using System.Threading.Tasks;
using Silverfly;

namespace Sample.JSON;

public static class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            var parsed = Parser.Parse<JsonGrammar>(input, useStatementsAtToplevel: true);
            
            Console.WriteLine(parsed.Tree.Accept(new PrintVisitor()));
        }
    }
}
