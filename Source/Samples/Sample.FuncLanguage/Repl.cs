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

        /*
        let reflectedColor = reflect(Color) // get all metadata about object (name, members)
        print(reflectedColor.name) //returns type name
        print(reflectedColor.members) // returns list of members
        print(reflectedColor.members.0.name) // returns name of member 0 -> R
        print(reflectedColor.members.0.value) // returns value of member 0 -> 0
        */
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

            var parsed = Parser.Parse<ExpressionGrammar>(input);
            var rewritten = parsed.Tree.Accept(new RewriterVisitor());

            var evaluated = rewritten.Accept(new EvaluationVisitor(), Scope.Root);

            parsed.Document.PrintMessages();

            //Console.WriteLine("> " + evaluated);
        }
    }
}
