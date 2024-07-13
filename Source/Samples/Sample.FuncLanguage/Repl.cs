using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func;

public static class Repl
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

            var evaluated = rewritten.Accept(new EvaluationVisitor(), Scope.Root);

            parsed.Document.PrintMessages();

            //Console.WriteLine("> " + evaluated);
        }
    }
}
