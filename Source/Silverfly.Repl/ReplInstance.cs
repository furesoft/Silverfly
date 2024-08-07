using PrettyPrompt;
using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;
using static System.ConsoleKey;
using static System.ConsoleModifiers;

namespace Silverfly.Repl;

public abstract class ReplInstance<TParser>(PromptCallbacks funcPromptCallbacks)
    where TParser : Parser, new()
{
    protected readonly TParser Parser = new();

    public abstract void Evaluate(string input);

    public async void Run()
    {
        await using var prompt = new Prompt(
            persistentHistoryFilepath: "./history-file",
            callbacks: funcPromptCallbacks,
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

            if (!response.IsSuccess) // false if user cancels, i.e. ctrl-c
            {
                continue;
            }

            if (response.Text == "exit") break;

            Evaluate(response.Text);

            Parser.PrintMessages();
        }
    }
}
