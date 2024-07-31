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

    private readonly string[] _keywords = ["let", "if", "then", "else"];

    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret, TextSpan spanToBeReplaced, CancellationToken cancellationToken)
    {
        // demo completion algorithm callback
        // populate completions and documentation for autocompletion window
        var typedWord = text.AsSpan(spanToBeReplaced.Start, spanToBeReplaced.Length).ToString();

        return Task.FromResult<IReadOnlyList<CompletionItem>>(
            _keywords
                .Select(kw =>
                {
                    var displayText = new FormattedString(kw, new FormatSpan(0, kw.Length, AnsiColor.Blue));

                    return new CompletionItem(
                        replacementText: kw,
                        displayText: displayText,
                        commitCharacterRules: [..new[] { new CharacterSetModificationRule(CharacterSetModificationKind.Add, [..new[] { ' ' }]) }]
                    );
                })
                .ToArray()
        );
    }

    protected override async Task<IReadOnlyCollection<FormatSpan>> HighlightCallbackAsync(string text, CancellationToken cancellationToken)
    {
        return EnumerateFormatSpans(text, _keywords.Select(f => (f, AnsiColor.Blue))).ToList();;
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
