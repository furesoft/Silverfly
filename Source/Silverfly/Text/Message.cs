namespace Silverfly.Text;

/// <summary>Represents an error message for lexing or parsing</summary>
public sealed class Message(MessageSeverity severity, string text, SourceRange range)
{
    public SourceRange Range { get; } = range;

    public SourceDocument Document { get; } = range.Document;

    public MessageSeverity Severity { get; } = severity;
    public string Text { get; } = text;

    public static Message Error(string message, SourceRange range)
    {
        return new(MessageSeverity.Error, message, range);
    }

    public static Message Error(string message)
    {
        return new(MessageSeverity.Error, message, SourceRange.Empty);
    }

    public static Message Info(string message, SourceRange range)
    {
        return new(MessageSeverity.Info, message, range);
    }

    public static Message Warning(string message, SourceRange range)
    {
        return new(MessageSeverity.Warning, message, range);
    }

    public override string ToString()
    {
        if (Document == null) return Text;

        return $"{Document.Filename}:{Range.Start.Line}:{Range.Start.Column} {Text}";
    }
}
