using Sample.FuncLanguage.Values;
using Silverfly;

namespace Sample.FuncLanguage;

public class Program
{
    public static void Main(string[] args)
    {
        Scope.Root.Define("print", (Value x) =>
        {
            Console.WriteLine(x);

            return UnitValue.Shared;
        });

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            var parsed = Parser.Parse<ExpressionGrammar>(input);
            var rewritten = parsed.Tree.Accept(new RewriterVisitor());

            //Console.WriteLine("Old: ");
            //Console.WriteLine(parsed.Tree.Accept(new PrintVisitor()));

            //Console.WriteLine("New: ");
            //Console.WriteLine(rewritten.Accept(new PrintVisitor()));
            var evaluated = rewritten.Accept(new EvaluationVisitor(), Scope.Root);

            parsed.Document.PrintMessages();

            //Console.WriteLine("> " + evaluated);
        }
    }
}
