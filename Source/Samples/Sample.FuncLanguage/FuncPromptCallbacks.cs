using System.Collections.Immutable;
using PrettyPrompt;
using PrettyPrompt.Completion;
using PrettyPrompt.Consoles;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;

namespace Silverfly.Sample.Func;

internal class FuncPromptCallbacks : PromptCallbacks
{
    /*
    protected override IEnumerable<(KeyPressPattern Pattern, KeyPressCallbackAsync Callback)> GetKeyPressCallbacks()
    {
        // registers functions to be called when the user presses a key. The text
        // currently typed into the prompt, along with the caret position within
        // that text are provided as callback parameters.
        yield return (new(ConsoleModifiers.Control, ConsoleKey.F1), ShowFruitDocumentation);
    }
    */

    private readonly string[] _keywords = ["let", "if", "then", "else", "import"];

    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret, TextSpan spanToBeReplaced, CancellationToken cancellationToken)
    {
        var typedWord = text.AsSpan(spanToBeReplaced.Start, spanToBeReplaced.Length).ToString();
        var namesInScope = Scope.Root.Bindings.Keys;

        return Task.FromResult<IReadOnlyList<CompletionItem>>(
            _keywords.Concat(namesInScope).Concat(["std"])
                .Where(_ => _.StartsWith(typedWord, StringComparison.InvariantCultureIgnoreCase))
                .Select(_ =>
                {
                    var displayText = new FormattedString(_,
                        new FormatSpan(0, _.Length, AnsiColor.Blue));

                    return new CompletionItem(
                        replacementText: _,
                        displayText: displayText,
                        commitCharacterRules: [..new[]
                        {
                            new CharacterSetModificationRule(CharacterSetModificationKind.Add,
                                [..new[] { ' ' }])
                        }]
                    );
                })
                .ToArray()
        );
    }

    protected override async Task<IReadOnlyCollection<FormatSpan>> HighlightCallbackAsync(string text, CancellationToken cancellationToken)
    {
        return EnumerateFormatSpans(text, _keywords.Select(f => (f, AnsiColor.Blue))).ToList();
    }

    private static IEnumerable<FormatSpan> EnumerateFormatSpans(string text, IEnumerable<(string TextToFormat, AnsiColor Color)> formattingInfo)
    {
        foreach (var (textToFormat, color) in formattingInfo)
        {
            int startIndex;
            int offset = 0;

            while ((startIndex = text.AsSpan(offset).IndexOf(textToFormat)) != -1)
            {
                yield return new FormatSpan(offset + startIndex, textToFormat.Length, color);
                offset += startIndex + textToFormat.Length;
            }
        }
    }
}
