using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Silverfly.Text.Formatting.Themes;

namespace Silverfly.Text.Formatting;

//ToDo: implement bracket matching coloring

/// <summary>
/// Provides methods to format and display compiler messages with syntax highlighting and theming.
/// </summary>
public partial class MessageFormatter(Parser parser)
{
    internal static FormatterTheme Theme = new DefaultFormatterTheme();

    /// <summary>
    /// Sets the theme for highlighting source code.
    /// </summary>
    /// <param name="theme">The new theme to be used.</param>
    public static void SetTheme(FormatterTheme theme)
    {
        Theme = theme;
    }

    void Write(string src, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(src);
        Theme.Reset();
    }

    void WriteLine(string src, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(src);
        Theme.Reset();
    }

    internal void PrintError(CompilerError error)
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

    private void WriteUnderline(CompilerError error)
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

    private void WriteMessage(CompilerError error)
    {
        var severityColor = GetColorForSeverity(error.Message.Severity);
        Write("    | ", Theme.Border);

        Write($"{error.Message.Severity}", severityColor);
        WriteLine($": {error.Message.Text}", Theme.Foreground);
    }

    private ConsoleColor GetColorForSeverity(MessageSeverity severity)
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

    //Todo: add loading keywords from grammar
    private void WriteHighlightedSource(string line)
    {
        string[] keywords = ["using", "class", "static", "void", "Console", "WriteLine"];

        var currentIndex = 0;
        var openBrackets = new Stack<ConsoleColor>();

        while (currentIndex < line.Length)
        {
            var isKeyword = false;

            // Highlight keywords
            foreach (var keyword in keywords)
            {
                if (!line[currentIndex..].StartsWith(keyword) ||
                    (currentIndex + keyword.Length != line.Length &&
                     char.IsLetterOrDigit(line[currentIndex + keyword.Length])))
                {
                    continue;
                }
                
                Write(keyword, Theme.Keyword);

                currentIndex += keyword.Length;
                isKeyword = true;
                break;
            }

            if (isKeyword)
            {
                continue;
            }

            // Highlight string literals
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
            // Highlight numbers
            else
            {
                var match = NumberRegex().Match(line[currentIndex..]);
                
                if (match.Success)
                {
                    Console.ForegroundColor = Theme.Number;
                    Console.Write(match.Value);

                    currentIndex += match.Value.Length;
                }
                // Highlight parentheses and braces
                else if (IsOpenBracket(line, currentIndex))
                {
                    var color = GetNextBracketColor();
                    openBrackets.Push(color);

                    Console.ForegroundColor = color;
                    Console.Write(line[currentIndex]);
                    currentIndex++;
                }
                else if (IsClosingBracket(line, currentIndex))
                {
                    if (openBrackets.Count > 0)
                    {
                        var openingBracket = openBrackets.Pop();

                        // Highlight matching pairs
                        Console.ForegroundColor = openingBracket;
                        Console.Write(line[currentIndex]);
                    }
                    else
                    {
                        // No matching opening bracket, just print it normally
                        Console.Write(line[currentIndex]);
                    }

                    currentIndex++;
                }
                else
                {
                    // Default color
                    Theme.Reset();
                    Console.Write(line[currentIndex]);
                    currentIndex++;
                }
            }
        }

        Console.WriteLine();
        Theme.Reset();
    }

    private bool IsClosingBracket(string line, int currentIndex)
    {
        return line[currentIndex] == ')' || line[currentIndex] == '}' || line[currentIndex] == '>' || line[currentIndex] == '[';
    }

    private bool IsOpenBracket(string line, int currentIndex)
    {
        return line[currentIndex] == '(' || line[currentIndex] == '{' || line[currentIndex] == '<' || line[currentIndex] == ']';
    }

    private int _currentBracketColorIndex = 0;
    private ConsoleColor GetNextBracketColor()
    {
        return Theme.BracketColors[_currentBracketColorIndex++ % Theme.BracketColors.Length];
    }

    [GeneratedRegex(@"^(0x[0-9A-Fa-f]+|0b[01]+|\d+)")]
    private static partial Regex NumberRegex();
}
