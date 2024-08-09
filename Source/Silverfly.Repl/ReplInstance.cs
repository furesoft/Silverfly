using PrettyPrompt;
using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;
using static System.ConsoleKey;
using static System.ConsoleModifiers;

namespace Silverfly.Repl;

public abstract class ReplInstance<TParser, TCallbacks>()
    where TParser : Parser, new()
    where TCallbacks : ReplPromptCallbacks, new()
{
    protected readonly TParser Parser = new();

    public abstract void Evaluate(string input);

    public async void Run()
    {
        var callbacks = new TCallbacks();
        callbacks.Parser = Parser;
        
        await using var prompt = new Prompt(
            persistentHistoryFilepath: "./history-file",
            callbacks: callbacks,
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
