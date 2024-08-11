using System;
using System.Text.RegularExpressions;

namespace Silverfly.Text.Formatting.Messages;

public partial class MessageFormatter
{
    [GeneratedRegex(@"^(0x[0-9A-Fa-f]+|0b[01]+|\d+)")]
    private static partial Regex NumberRegex();
    
    private void HighlightString(string line, ref int currentIndex, Symbol start, Symbol end)
    {
        if (IsMatch(start, line, ref currentIndex))
        {
            var closingQuoteIndex = line.IndexOf(end.Name, currentIndex + 1, StringComparison.Ordinal);
            if (closingQuoteIndex == -1)
            {
                closingQuoteIndex = line.Length - 1;
            }
            
            Write(line.Substring(currentIndex, closingQuoteIndex - currentIndex + 1), Theme.String);

            currentIndex = closingQuoteIndex + 1;
        }
    }
    
    private static void HighlightNumber(string line, ref int currentIndex)
    {
        var match = NumberRegex().Match(line[currentIndex..]);

        if (match.Success)
        {
            Console.ForegroundColor = Theme.Number;
            Console.Write(match.Value);
            
            currentIndex += match.Value.Length;
        }
    }
    
    public static bool IsClosingBracket(char c)
    {
        return c is ')' or '}' or '>' or ']';
    }

    public static bool IsOpenBracket(char c)
    {
        return c is '(' or '{' or '<' or '[';
    }

    private int _currentBracketColorIndex = 0;
    private ConsoleColor GetNextBracketColor()
    {
        return Theme.BracketColors[_currentBracketColorIndex++ % Theme.BracketColors.Length];
    }
}
