using PrettyPrompt;
using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;
using Silverfly.Sample.Func.Values;
using Silverfly.Text;
using static System.ConsoleKey;
using static System.ConsoleModifiers;

namespace Silverfly.Sample.Func;

public static class Repl
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

            return new ModuleValue(scope);
        });

        await using var prompt = new Prompt(
            persistentHistoryFilepath: "./history-file",
            callbacks: new FuncPromptCallbacks(),
            configuration: new PromptConfiguration(
                prompt: new FormattedString("> "),
                keyBindings: new KeyBindings(
                    commitCompletion: new[] { new KeyPressPattern(Tab) },
                    submitPrompt: new[] { new KeyPressPattern(Enter) },
                    newLine: new[] { new KeyPressPattern(Shift, Enter) },
                    triggerOverloadList: new(new KeyPressPattern('('))
                ),
                completionItemDescriptionPaneBackground: AnsiColor.Rgb(30, 30, 30),
                selectedCompletionItemBackground: AnsiColor.Rgb(30, 30, 30),
                selectedTextBackground: AnsiColor.Rgb(20, 61, 102)));

        while (true)
        {
            var response = await prompt.ReadLineAsync();
            if (response.IsSuccess) // false if user cancels, i.e. ctrl-c
            {
                if (response.Text == "exit") break;

                var parsed = new ExpressionGrammar().Parse(response.Text, "repl.f");
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
}
