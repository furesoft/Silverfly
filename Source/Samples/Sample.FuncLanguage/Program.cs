using Silverfly.Sample.Func.Values;


namespace Silverfly.Sample.Func;

public static class Program
{
    public static async Task Main(string[] args)
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
            scope.Define("annotations", Value.From(x.Annotations));
            
            scope.Define("hasAnnotation", nameValue =>
            {
                var name = (StringValue)nameValue;

                return x.Annotations.Any(a => a.Name == name.Value);
            });

            return new ModuleValue(scope);
        });

        new Repl().Run();
    }
}
