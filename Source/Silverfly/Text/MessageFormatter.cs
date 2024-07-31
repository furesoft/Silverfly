using System;

namespace Silverfly.Text;

internal static class MessageFormatter
{
    public static void PrintError(CompilerError error)
    {
        Console.WriteLine($"    ┌─ {error.Message.Document.Filename}");

        for (var i = 0; i < error.SourceLines.Count; i++)
        {
            var lineNumber = error.Message.Range.Start.Line + i;
            var lineContent = error.SourceLines[i];

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"  {lineNumber} ");
            Console.ResetColor();

            Console.Write("│ ");
            HighlightSyntax(lineContent);

            if (error.HighlightLines.Contains(error.Message.Range.Start.Line))
            {
                var highlightIndex = error.Message.Range.Start.Column; // Default to column if no caret

                var underline = new string(' ', highlightIndex) + new string('~', error.Message.Range.End.Column - highlightIndex);
                Console.WriteLine($"     {underline}^");
            }

            Console.ForegroundColor = GetColorForSeverity(error.Message.Severity);
            Console.Write($"      {error.Message.Severity}");
            Console.ResetColor();
            Console.WriteLine($": {error.Message.Text}");
        }

        Console.WriteLine("    ");
    }

    private static ConsoleColor GetColorForSeverity(MessageSeverity severity)
    {
        return severity switch
        {
            MessageSeverity.Error => ConsoleColor.Red,
            MessageSeverity.Warning => ConsoleColor.Yellow,
            MessageSeverity.Info => ConsoleColor.DarkGreen,
            MessageSeverity.Hint => ConsoleColor.DarkYellow,
        };
    }

    static void HighlightSyntax(string code)
    {
        string[] keywords = ["using", "class", "static", "void", "Console", "WriteLine"];
        var keywordColor = ConsoleColor.Blue;
        var stringColor = ConsoleColor.Green;

        var lines = code.Split([Environment.NewLine], StringSplitOptions.None);

        foreach (var line in lines)
        {
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

                    Console.ForegroundColor = keywordColor;
                    Console.Write(keyword);
                    currentIndex += keyword.Length;
                    isKeyword = true;
                    break;
                }

                if (!isKeyword)
                {
                    if (line[currentIndex] == '"')
                    {
                        var closingQuoteIndex = line.IndexOf('"', currentIndex + 1);
                        if (closingQuoteIndex == -1)
                        {
                            closingQuoteIndex = line.Length - 1;
                        }

                        Console.ForegroundColor = stringColor;
                        Console.Write(line.Substring(currentIndex, closingQuoteIndex - currentIndex + 1));
                        currentIndex = closingQuoteIndex + 1;
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(line[currentIndex]);
                        currentIndex++;
                    }
                }
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}
