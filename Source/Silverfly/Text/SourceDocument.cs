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

    public void PrintMessages()
    {
        foreach (var message in Messages)
        {
            MessageFormatter.PrintError(CompilerError.FromMessage(message));
        }

        Console.ResetColor();
    }

    public void AddMessage(MessageSeverity messageSeverity, string message, SourceRange range)
    {
        Messages.Add(new Message(messageSeverity, message, range));
    }

    public void AddMessage(MessageSeverity messageSeverity, string message, int startLine, int startColumn, int endLine, int endColumn)
    {
        Messages.Add(new Message(messageSeverity, message, SourceRange.From(this, startLine, startColumn, endLine, endColumn)));
    }
}
