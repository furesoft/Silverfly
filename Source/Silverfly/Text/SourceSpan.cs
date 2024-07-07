namespace Silverfly.Text;

/// <summary>Represents a part of the source file</summary>
/// <summary>
/// Represents a position within a source document defined by line and column numbers.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceSpan"/> struct with the specified line and column numbers.
/// </remarks>
/// <param name="line">The line number.</param>
/// <param name="column">The column number.</param>
public readonly struct SourceSpan(int line, int column)
{
    /// <summary>
    /// Gets the line number.
    /// </summary>
    public int Line { get; } = line;

    /// <summary>
    /// Gets the column number.
    /// </summary>
    public int Column { get; } = column;

    /// <summary>
    /// Determines if one <see cref="SourceSpan"/> is less than another.
    /// </summary>
    /// <param name="left">The left <see cref="SourceSpan"/>.</param>
    /// <param name="right">The right <see cref="SourceSpan"/>.</param>
    /// <returns>True if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, false.</returns>
    public static bool operator <(SourceSpan left, SourceSpan right)
    {
        if (left.Line < right.Line)
        {
            return true;
        }

        return left.Line == right.Line &&
               left.Column < right.Column;
    }

    /// <summary>
    /// Determines if one <see cref="SourceSpan"/> is greater than another.
    /// </summary>
    /// <param name="left">The left <see cref="SourceSpan"/>.</param>
    /// <param name="right">The right <see cref="SourceSpan"/>.</param>
    /// <returns>True if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, false.</returns>
    public static bool operator >(SourceSpan left, SourceSpan right)
    {
        return !(left < right);
    }

    /// <summary>
    /// Returns a string representation of the <see cref="SourceSpan"/> in the format: "{Line}:{Column}".
    /// </summary>
    /// <returns>A string representation of the <see cref="SourceSpan"/>.</returns>
    public override string ToString()
    {
        return $"{Line}:{Column}";
    }
}
