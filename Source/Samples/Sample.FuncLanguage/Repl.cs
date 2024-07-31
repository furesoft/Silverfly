using Silverfly.Nodes;
using Silverfly.Sample.Func.Values;
using Silverfly.Text;

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

        OptionValue.AddToScope();

        Scope.Root.Define("reflect", (Value x) =>
        {
            var scope = new Scope();

            scope.Define("name", x.Members.Get("__name___"));

            var members = new List<Value>();
            foreach (var member in x.Members.Bindings)
            {
                var memberScope = new Scope();
                memberScope.Define("name", member.Key);
                memberScope.Define("value", member.Value);

                members.Add(new ModuleValue(memberScope));
            }

            scope.Define("members", new ListValue(members));

            return new ModuleValue(scope);
        });

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            var parsed = Parser.Parse<ExpressionGrammar>(input, "repl.f");
            var rewritten = parsed.Tree.Accept(new RewriterVisitor());

            //Console.WriteLine(rewritten.Accept(new PrintVisitor()));

            if (!parsed.Document.Messages.Any())
            {
                var evaluated = rewritten.Accept(new EvaluationVisitor(), Scope.Root);

                if (evaluated is NameValue n)
                {
                    rewritten.AddMessage(MessageSeverity.Error, $"Symbol '{n.Name}' not defined");
                }
            }

            parsed.Document.PrintMessages();

            //Console.WriteLine("> " + evaluated);
        }
    }
}
