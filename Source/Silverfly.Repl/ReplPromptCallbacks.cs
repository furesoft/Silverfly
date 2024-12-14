using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PrettyPrompt;
using PrettyPrompt.Highlighting;
using Silverfly.Text.Formatting;

namespace Silverfly.Repl;

public class ReplPromptCallbacks : PromptCallbacks
{
    protected static readonly char[] Characters = [' '];
    public Parser Parser { get; set; }

    protected override Task<IReadOnlyCollection<FormatSpan>> HighlightCallbackAsync(string text,
        CancellationToken cancellationToken)
    {
        var keywords = Parser.Lexer.Config.Keywords.Select(f => (f, ToAnsi(MessageFormatter.Theme.Keyword)));
        var brackets = GetBracketSpans(text);

        var spans = GetKeywordSpans(text, keywords)
            .Concat(brackets)
            .Concat(GetNumberSpans(text))
            .Concat(GetStringsSpans(text))
            .ToList();

        return Task.FromResult<IReadOnlyCollection<FormatSpan>>(spans);
    }

    protected static AnsiColor ToAnsi(ConsoleColor c)
    {
        _ = AnsiColor.TryParse(c.ToString(), out var color);

        return color;
    }

    private static IEnumerable<FormatSpan> GetBracketSpans(string text)
    {
        var bracketPairs = new Dictionary<char, char> { { '(', ')' }, { '[', ']' }, { '{', '}' } };

        var stack = new Stack<(AnsiColor Color, int Index)>();
        var colorIndex = 0;

        for (var i = 0; i < text.Length; i++)
        {
            if (MessageFormatter.IsOpenBracket(text[i]))
            {
                var consoleColor =
                    MessageFormatter.Theme.BracketColors[colorIndex % MessageFormatter.Theme.BracketColors.Length];
                colorIndex++;

                stack.Push((ToAnsi(consoleColor), i));
            }
            else if (bracketPairs.ContainsValue(text[i]))
            {
                if (stack.Count > 0 && MessageFormatter.IsClosingBracket(text[i]))
                {
                    var (color, startIndex) = stack.Pop();

                    yield return new FormatSpan(startIndex, 1, color); // Opening bracket
                    yield return new FormatSpan(i, 1, color); // Closing bracket
                }
            }
        }
    }


    private IEnumerable<FormatSpan> GetKeywordSpans(string text,
        IEnumerable<(string TextToFormat, AnsiColor Color)> formattingInfo)
    {
        foreach (var (textToFormat, color) in formattingInfo)
        {
            int startIndex;
            var offset = 0;

            while ((startIndex = text.AsSpan(offset).ToString().IndexOf(textToFormat, Parser.Lexer.Config.Casing)) !=
                   -1)
            {
                yield return new FormatSpan(offset + startIndex, textToFormat.Length, color);
                offset += startIndex + textToFormat.Length;
            }
        }
    }

    //ToDo: use highlighting information from error highlighter
    private static IEnumerable<FormatSpan> GetStringsSpans(string text)
    {
        var offset = 0;

        while (offset < text.Length)
        {
            var startIndex = text.IndexOf('"', offset);
            if (startIndex == -1)
            {
                break;
            }

            var endIndex = text.IndexOf('"', startIndex + 1);
            if (endIndex == -1)
            {
                break;
            }

            var length = endIndex - startIndex + 1;
            yield return new FormatSpan(startIndex, length, ToAnsi(MessageFormatter.Theme.String));

            offset = endIndex + 1;
        }
    }

    private static IEnumerable<FormatSpan> GetNumberSpans(string text)
    {
        var offset = 0;

        while (offset < text.Length)
        {
            if (char.IsDigit(text[offset]))
            {
                var startIndex = offset;

                while (offset < text.Length && char.IsDigit(text[offset]))
                {
                    offset++;
                }

                if (startIndex == 0 || !char.IsLetter(text[startIndex - 1]))
                {
                    yield return new FormatSpan(startIndex, offset, ToAnsi(MessageFormatter.Theme.Number));
                }
            }
            offset++;
        }
    }
}
