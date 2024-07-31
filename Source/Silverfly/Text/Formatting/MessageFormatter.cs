using System;
using System.Text.RegularExpressions;
using Silverfly.Text.Formatting.Themes;

namespace Silverfly.Text.Formatting;

public static class MessageFormatter
{
    internal static FormatterTheme Theme = new DefaultFormatterTheme();

    public static void SetTheme(FormatterTheme theme)
    {
        Theme = theme;
    }

    static void Write(string src, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(src);
        Theme.Reset();
    }

    static void WriteLine(string src, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(src);
        Theme.Reset();
    }

    internal static void PrintError(CompilerError error)
    {
        Console.BackgroundColor = Theme.Background;

        Write("    ┌─ ", Theme.Border);

        WriteLine(error.Message.Document.Filename, Theme.Filename);

        for (var i = 0; i < error.SourceLines.Count; i++)
        {
            var lineNumber = error.Message.Range.Start.Line + i;
            var lineContent = error.SourceLines[i];

            Write($"  {lineNumber} ", Theme.LineNumber);

            Write("│ ", Theme.Border);
            WriteHighlightedSource(lineContent);

            WriteUnderline(error);

            WriteMessage(error);
        }

        Write("    \u2514─ ", Theme.Border);

        Console.WriteLine("    ");
    }

    private static void WriteUnderline(CompilerError error)
    {
        if (!error.HighlightLines.Contains(error.Message.Range.Start.Line))
        {
            return;
        }

        var highlightIndex = error.Message.Range.Start.Column;

        var underline = new string(' ', highlightIndex) +
                        new string('~', error.Message.Range.End.Column - highlightIndex);

        WriteLine($"    |{underline}^", Theme.Underline);
    }

    private static void WriteMessage(CompilerError error)
    {
        var severityColor = GetColorForSeverity(error.Message.Severity);
        Write("    | ", Theme.Border);

        Write($"{error.Message.Severity}", severityColor);
        WriteLine($": {error.Message.Text}", Theme.Foreground);
    }

    private static ConsoleColor GetColorForSeverity(MessageSeverity severity)
    {
        return severity switch
        {
            MessageSeverity.Error => Theme.SeverityError,
            MessageSeverity.Warning => Theme.SeverityWarning,
            MessageSeverity.Info => Theme.SeverityInfo,
            MessageSeverity.Hint => Theme.SeverityHint,
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    }

    static void WriteHighlightedSource(string line)
    {
        string[] keywords = ["using", "class", "static", "void", "Console", "WriteLine"];

        var currentIndex = 0;
        while (currentIndex < line.Length)
        {
            var isKeyword = false;

            foreach (var keyword in keywords)
            {
                if (!line[currentIndex..].StartsWith(keyword) ||
                    (currentIndex + keyword.Length != line.Length &&
                     char.IsLetterOrDigit(line[currentIndex + keyword.Length])))
                {
                    continue;
                }

                Console.ForegroundColor = Theme.Keyword;
                Console.Write(keyword);

                currentIndex += keyword.Length;
                isKeyword = true;
                break;
            }

            if (isKeyword)
            {
                continue;
            }

            if (line[currentIndex] == '"')
            {
                var closingQuoteIndex = line.IndexOf('"', currentIndex + 1);
                if (closingQuoteIndex == -1)
                {
                    closingQuoteIndex = line.Length - 1;
                }

                Console.ForegroundColor = Theme.String;
                Console.Write(line.Substring(currentIndex, closingQuoteIndex - currentIndex + 1));

                currentIndex = closingQuoteIndex + 1;
            }
            else
            {
                var match = Regex.Match(line[currentIndex..], @"^(0x[0-9A-Fa-f]+|0b[01]+|\d+)");
                if (match.Success)
                {
                    Console.ForegroundColor = Theme.Number;
                    Console.Write(match.Value);

                    currentIndex += match.Value.Length;
                }
                else
                {
                    Theme.Reset();

                    Console.Write(line[currentIndex]);
                    currentIndex++;
                }
            }
        }

        Console.WriteLine();

        Theme.Reset();
    }
}
