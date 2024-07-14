using System;

namespace Silverfly.Text;

internal static class MessageFormatter
{
    public static void PrintError(CompilerError error)
    {
        Console.WriteLine($"   ┌─ {error.Message.Document.Filename}:{error.Message.Range.Start.Line}:{error.Message.Range.Start.Column}");

        for (var i = 0; i < error.SourceLines.Count; i++)
        {
            var lineNumber = error.Message.Range.Start.Line + i;
            var lineContent = error.SourceLines[i];

            if (error.HighlightLines.Contains(error.Message.Range.Start.Line))
            {
                Console.WriteLine($"  {lineNumber} │ {lineContent}");
                var highlightIndex = error.Message.Range.Start.Column; // Default to column if no caret

                var underline = new string(' ', highlightIndex) + "╭─" + new string('─', lineContent.Length - highlightIndex - 1) + "^";
                Console.WriteLine($"  {underline}");
            }
            else
            {
                Console.WriteLine($"  {lineNumber} │ {lineContent}");
            }

            Console.WriteLine($"      {error.Message.Severity}: {error.Message.Text}");
        }

        Console.WriteLine("    ·");
    }
}