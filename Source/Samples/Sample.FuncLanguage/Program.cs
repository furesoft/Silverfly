using Silverfly.Sample.Func.Values;


namespace Silverfly.Sample.Func;

public static class Program
{
    public static Task Main(string[] args)
    {
        Scope.Root.Define("print", new Action<object>(x =>
        {
            if ((Value)x == UnitValue.Shared)
            {
                return;
            }

            Console.WriteLine(x);
        }));

        OptionValue.AddToScope();

        Scope.Root.Define("reflect", new Func<Value, ModuleValue>(x =>
        {
            var scope = new Scope();

            scope.Define("name", x.Members.Get("__name___") ?? GetPrimitiveName(x));

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
            
            scope.Define("hasAnnotation", new Func<string, bool>(name =>
            {
                return x.Annotations.Any(a => a.Name == name);
            }));

            return new ModuleValue(scope);
        }));
        
        Scope.Root.Define("greet", Value.From(new Func<Value>(() => null)));

        new Repl().Run();
        return Task.CompletedTask;
    }

    private static Value GetPrimitiveName(Value value)
    {
        return value switch
        {
            StringValue => "string",
            NumberValue => "number",
            OptionValue => "option[]",
            ListValue => "list",
            UnitValue => "unit",
            LambdaValue => "lambda",
            _ => "no name"
        };
    }
}
