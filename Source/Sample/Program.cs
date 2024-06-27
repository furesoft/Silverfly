using Furesoft.PrattParser;

namespace Sample;

public class Program
{
    public static void Main(string[] args)
    {
        Scope.Root.Define("+", (a, b) => a + b);

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            var parsed = Parser.Parse<ExpressionGrammar>(input);
            var rewritten = parsed.Tree.Accept(new RewriterVisitor());

            Console.WriteLine("Old: ");
            Console.WriteLine(parsed.Tree.Accept(new PrintVisitor()));

            Console.WriteLine("New: ");
            Console.WriteLine(rewritten.Accept(new PrintVisitor()));

            var evaluated = rewritten.Accept(new EvaluationVisitor());
            Console.WriteLine("> " + evaluated);
        }
    }
}
