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

            var parsed = new JsonGrammar().Parse(input);
            
            Console.WriteLine(parsed.Tree.Accept(new PrintVisitor()));
        }
    }
}
