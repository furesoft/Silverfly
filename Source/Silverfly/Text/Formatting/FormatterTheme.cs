using System;

namespace Silverfly.Text.Formatting;

public abstract class FormatterTheme
{
    public ConsoleColor Background { get; protected init; }
    public ConsoleColor Foreground { get; protected init; }
    public ConsoleColor LineNumber { get; protected init; }
    public ConsoleColor Underline { get; protected init; }
    public ConsoleColor Border { get; protected init; }
    public ConsoleColor Filename { get; protected init; }

    public ConsoleColor SeverityError { get; protected init; }
    public ConsoleColor SeverityWarning { get; protected init; }
    public ConsoleColor SeverityInfo { get; protected init; }
    public ConsoleColor SeverityHint { get; protected init; }

    public ConsoleColor Number { get; protected init; }
    public ConsoleColor Keyword { get; protected init; }
    public ConsoleColor String { get; protected init; }

    public void Reset()
    {
        Console.BackgroundColor = Background;
        Console.ForegroundColor = Foreground;
    }
}
