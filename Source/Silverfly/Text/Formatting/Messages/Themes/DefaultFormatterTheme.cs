using System;

namespace Silverfly.Text.Formatting.Messages.Themes;

public class DefaultFormatterTheme : FormatterTheme
{
    public DefaultFormatterTheme()
    {
        Background = ConsoleColor.Black;
        Foreground = ConsoleColor.Gray;
        LineNumber = ConsoleColor.DarkGray;
        Underline = Foreground;
        Border = Foreground;
        Filename = Foreground;

        SeverityError = ConsoleColor.Red;
        SeverityWarning = ConsoleColor.Yellow;
        SeverityInfo = ConsoleColor.DarkGreen;
        SeverityHint = ConsoleColor.DarkYellow;

        Number = ConsoleColor.Magenta;
        String = ConsoleColor.Yellow;
        Keyword = ConsoleColor.Blue;
    }
}
