using System;
using System.Collections.Generic;
using System.Linq;
using Silverfly.Lexing.Matcher;
using Silverfly.Text.Formatting.Themes;

namespace Silverfly.Text.Formatting;

/// <summary>
/// Provides methods to format and display compiler messages with syntax highlighting and theming.
/// </summary>
public partial class MessageFormatter(Parser parser)
{
    public static FormatterTheme Theme = new DefaultFormatterTheme();

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

    public bool IsMatch(Symbol token, string line, ref int currentIndex)
    {
        if (string.IsNullOrEmpty(token.Name))
        {
            return false;
        }

        if (currentIndex + token.Name.Length > line.Length)
        {
            return false;
        }

        var nameSpan = token.Name.AsMemory().Span;
        var documentSliceSpan = line.AsSpan().Slice(currentIndex, token.Name.Length);
        
        return nameSpan.CompareTo(documentSliceSpan, StringComparison.Ordinal) == 0;
    }
    
    private void WriteHighlightedSource(string line)
    {
        var currentIndex = 0;
        var openBrackets = new Stack<ConsoleColor>();

        while (currentIndex < line.Length)
        {
            var isKeyword = false;

            // Highlight keywords
            foreach (var keyword in parser.Lexer.Config.Keywords)
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

            foreach (var stringMatcher in parser.Lexer.Config.Matchers.OfType<StringMatcher>())
            {
                HighlightString(line, ref currentIndex, stringMatcher.Left, stringMatcher.Right);
            }

            if (parser.Lexer.Config.Matchers.OfType<NumberMatcher>().Any())
            {
                HighlightNumber(line, ref currentIndex);
            }

            if (currentIndex >= line.Length)
            {
                break;
            }
            
            // Highlight parentheses and braces
            if (IsOpenBracket(line[currentIndex]))
            {
                var color = GetNextBracketColor();
                openBrackets.Push(color);

                Console.ForegroundColor = color;
                Console.Write(line[currentIndex]);
                currentIndex++;
            }
            else if (IsClosingBracket(line[currentIndex]))
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
                    Theme.Reset();
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

        Console.WriteLine();
        Theme.Reset();
    }
}
