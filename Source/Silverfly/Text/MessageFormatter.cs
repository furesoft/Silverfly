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

            Console.WriteLine($"│ {lineContent}");

            if (error.HighlightLines.Contains(error.Message.Range.Start.Line))
            {
                var highlightIndex = error.Message.Range.Start.Column; // Default to column if no caret

                var underline = new string(' ', highlightIndex) + new string('~', error.Message.Range.End.Column - highlightIndex);
                Console.WriteLine($"     {underline}^");
            }

            Console.WriteLine($"      {error.Message.Severity}: {error.Message.Text}");
        }

        Console.WriteLine("    ");
    }
}
