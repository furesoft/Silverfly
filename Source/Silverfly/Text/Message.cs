namespace Silverfly.Text;

/// <summary>Represents an error message for lexing or parsing</summary>
/// <summary>
/// Represents a message associated with a source code range and severity level.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Message"/> class with the specified severity, text, and range.
/// </remarks>
/// <param name="severity">The severity level of the message.</param>
/// <param name="text">The text content of the message.</param>
/// <param name="range">The source range where the message applies.</param>
public sealed class Message(MessageSeverity severity, string text, SourceRange range)
{
    /// <summary>
    /// Gets the range in the source code where the message applies.
    /// </summary>
    public SourceRange Range { get; } = range;

    /// <summary>
    /// Gets the source document associated with the message range.
    /// </summary>
    public SourceDocument Document { get; } = range.Document;

    /// <summary>
    /// Gets the severity level of the message.
    /// </summary>
    public MessageSeverity Severity { get; } = severity;

    /// <summary>
    /// Gets the text content of the message.
    /// </summary>
    public string Text { get; } = text;

    /// <summary>
    /// Constructs an error message with the specified severity, text, and range.
    /// </summary>
    /// <param name="message">The error message text.</param>
    /// <param name="range">The source range where the error occurred.</param>
    /// <returns>A new <see cref="Message"/> instance representing an error.</returns>
    public static Message Error(string message, SourceRange range)
    {
        return new Message(MessageSeverity.Error, message, range);
    }


    /// <summary>
    /// Constructs an information message with the specified severity, text, and range.
    /// </summary>
    /// <param name="message">The information message text.</param>
    /// <param name="range">The source range where the information applies.</param>
    /// <returns>A new <see cref="Message"/> instance representing an information message.</returns>
    public static Message Info(string message, SourceRange range)
    {
        return new Message(MessageSeverity.Info, message, range);
    }

    /// <summary>
    /// Constructs a warning message with the specified severity, text, and range.
    /// </summary>
    /// <param name="message">The warning message text.</param>
    /// <param name="range">The source range where the warning occurred.</param>
    /// <returns>A new <see cref="Message"/> instance representing a warning.</returns>
    public static Message Warning(string message, SourceRange range)
    {
        return new Message(MessageSeverity.Warning, message, range);
    }

    /// <summary>
    /// Returns a string representation of the message, including file location information if available.
    /// </summary>
    /// <returns>A string representation of the message.</returns>
    public override string ToString()
    {
        if (Document == null) return Text;

        return $"{Document.Filename}:{Range.Start.Line}:{Range.Start.Column} {Severity}: {Text}";
    }
}
