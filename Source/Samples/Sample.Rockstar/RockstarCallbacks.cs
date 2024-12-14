using PrettyPrompt.Completion;
using PrettyPrompt.Consoles;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;
using Silverfly.Repl;

namespace Silverfly.Sample.Rockstar;

public class RockstarCallbacks : ReplPromptCallbacks
{
    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret,
        TextSpan spanToBeReplaced, CancellationToken cancellationToken)
    {
        var samples = Directory.GetFiles("Samples");

        if (text != "samples") //toDo: fix completion
        {
            return Task.FromResult<IReadOnlyList<CompletionItem>>(Array.Empty<CompletionItem>());
        }

        return Task.FromResult<IReadOnlyList<CompletionItem>>(
            samples
                .Select(sample =>
                {
                    var displayText = new FormattedString(Path.GetFileName(sample),
                        new FormatSpan(0, sample.Length, AnsiColor.Blue));

                    return new CompletionItem(
                        File.ReadAllText(sample),
                        displayText,
                        commitCharacterRules:
                        [
                            ..new[]
                            {
                                new CharacterSetModificationRule(CharacterSetModificationKind.Add,
                                    [.. Characters])
                            }
                        ]
                    );
                })
                .ToArray()
        );
    }
}
