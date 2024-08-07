using System;
using System.Collections.Generic;
using Silverfly.Text.Formatting;

namespace Silverfly.Text;

/// <summary>
/// Represents a source document containing filename, source code, and associated messages.
/// </summary>
public class SourceDocument
{
    /// <summary>
    /// Gets or sets the filename of the source document.
    /// </summary>
    public string Filename { get; set; }

    /// <summary>
    /// Gets or sets the source code content as a read-only memory of characters.
    /// </summary>
    public ReadOnlyMemory<char> Source { get; set; }

    /// <summary>
    /// Gets the list of messages associated with the source document.
    /// </summary>
    public List<Message> Messages { get; } = [];

    /// <summary>
    /// Prints all messages stored in the message list using the <see cref="MessageFormatter"/>.
    /// </summary>
    public void PrintMessages()
    {
        foreach (var message in Messages)
        {
            MessageFormatter.PrintError(CompilerError.FromMessage(message));
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Adds a new message to the message list with the specified severity, text, and source range.
    /// </summary>
    /// <param name="messageSeverity">The severity of the message.</param>
    /// <param name="message">The text of the message.</param>
    /// <param name="range">The source range associated with the message.</param>
    public void AddMessage(MessageSeverity messageSeverity, string message, SourceRange range)
    {
        Messages.Add(new Message(messageSeverity, message, range));
    }

    /// <summary>
    /// Adds a new message to the message list with the specified severity, text, and source range defined by line and column numbers.
    /// </summary>
    /// <param name="messageSeverity">The severity of the message.</param>
    /// <param name="message">The text of the message.</param>
    /// <param name="startLine">The starting line number of the source range.</param>
    /// <param name="startColumn">The starting column number of the source range.</param>
    /// <param name="endLine">The ending line number of the source range.</param>
    /// <param name="endColumn">The ending column number of the source range.</param>
    public void AddMessage(MessageSeverity messageSeverity, string message, int startLine, int startColumn, int endLine, int endColumn)
    {
        AddMessage(messageSeverity, message, SourceRange.From(this, startLine, startColumn, endLine, endColumn));
    }
}
