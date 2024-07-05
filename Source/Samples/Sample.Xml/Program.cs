using System;
using System.Threading.Tasks;
using Silverfly;

namespace Sample.Xml;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            var parsed = Parser.Parse<XmlGrammar>(input);
            
            Console.WriteLine(parsed.Tree.Accept(new PrintVisitor()));
        }
    }
}