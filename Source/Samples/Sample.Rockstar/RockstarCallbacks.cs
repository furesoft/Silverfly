using PrettyPrompt.Completion;
using PrettyPrompt.Consoles;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;
using Silverfly.Repl;

namespace Sample.Rockstar;

public class RockstarCallbacks : ReplPromptCallbacks
{
    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret, TextSpan spanToBeReplaced, CancellationToken cancellationToken)
    {
        var samples = Directory.GetFiles("Samples");
        
        return Task.FromResult<IReadOnlyList<CompletionItem>>(
            samples
                .Select(sample =>
                {
                    var displayText = new FormattedString(Path.GetFileName(sample),
                        new FormatSpan(0, sample.Length, AnsiColor.Blue));

                    return new CompletionItem(
                        replacementText: File.ReadAllText(sample),
                        displayText: displayText,
                        commitCharacterRules: [..new[]
                        {
                            new CharacterSetModificationRule(CharacterSetModificationKind.Add,
                                [.. Characters])
                        }]
                    );
                })
                .ToArray()
        );
    }
}
