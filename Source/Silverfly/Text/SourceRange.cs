using System;

namespace Silverfly.Text;

/// <summary>
/// Represents a range within a source document defined by start and end positions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceRange"/> struct with the specified document, start, and end spans.
/// </remarks>
/// <param name="document">The source document associated with the range.</param>
/// <param name="start">The starting position of the range.</param>
/// <param name="end">The ending position of the range.</param>
public readonly struct SourceRange(SourceDocument document, SourceSpan start, SourceSpan end)
{
    /// <summary>
    /// Gets an empty source range.
    /// </summary>
    public static SourceRange Empty { get; } = new();

    /// <summary>
    /// Gets the source document associated with the range.
    /// </summary>
    public SourceDocument Document { get; } = document;

    /// <summary>
    /// Gets the starting position of the range.
    /// </summary>
    public SourceSpan Start { get; } = start;

    /// <summary>
    /// Gets the ending position of the range.
    /// </summary>
    public SourceSpan End { get; } = end;

    /// <summary>
    /// Determines if the specified line and column are contained within this source range.
    /// </summary>
    /// <param name="line">The line number to check.</param>
    /// <param name="column">The column number to check.</param>
    /// <returns>True if the line and column are within the range; otherwise, false.</returns>
    public bool Contains(int line, int column)
    {
        var inLeft = Start.Line < line || (Start.Line == line && Start.Column <= column);
        var inRight = End.Line > line || (End.Line == line && End.Column >= column);

        return inLeft && inRight;
    }

    /// <summary>
    /// Creates a new <see cref="SourceRange"/> instance from the specified parameters.
    /// </summary>
    /// <param name="document">The source document associated with the range.</param>
    /// <param name="startLine">The starting line number of the range.</param>
    /// <param name="startColumn">The starting column number of the range.</param>
    /// <param name="endLine">The ending line number of the range.</param>
    /// <param name="endColumn">The ending column number of the range.</param>
    /// <returns>A new <see cref="SourceRange"/> instance representing the specified range.</returns>
    public static SourceRange From(SourceDocument document, int startLine, int startColumn, int endLine, int endColumn)
    {
        return new SourceRange(document, new SourceSpan(startLine, startColumn), new SourceSpan(endLine, endColumn));
    }

    /// <summary>
    /// Retrieves a substring from the document source.
    /// </summary>
    /// <returns>A string representing the text within the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified range is out of the document lines.</exception>
    public string GetText()
    {
        var source = Document.Source.Span;
        var startLine = Start.Line - 1;
        var endLine = End.Line - 1;

        if (startLine < 0 || endLine >= CountLines(source))
            throw new IndexOutOfRangeException("Range is out of document lines.");

        var startIdx = GetLineStartIndex(source, startLine) + Start.Column - 1;
        var endIdx = GetLineStartIndex(source, endLine) + End.Column - 1;

        return new string(source[startIdx..endIdx]);
    }

    private int CountLines(ReadOnlySpan<char> source)
    {
        var lines = 0;
        for (var i = 0; i < source.Length; i++)
        {
            if (source[i] == '\n')
                lines++;
        }

        return lines + 1; // adding one because the last line may not end with a newline
    }

    private int GetLineStartIndex(ReadOnlySpan<char> source, int line)
    {
        var currentLine = 0;
        for (var i = 0; i < source.Length; i++)
        {
            if (currentLine == line)
                return i;
            if (source[i] == '\n')
                currentLine++;
        }

        throw new ArgumentOutOfRangeException(nameof(line), " line out of range.");
    }

    /// <summary>
    /// Returns a string representation of the source range in the format: "{Filename}: {Start} - {End}".
    /// </summary>
    /// <returns>A string representation of the source range.</returns>
    public override string ToString()
    {
        return $"{Document.Filename}: {Start} - {End}";
    }
}
