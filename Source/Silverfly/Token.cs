using System;
using Silverfly.Text;

namespace Silverfly;

/// <summary>
/// Represents a token in the lexer, which encapsulates a symbol type, text, line, column, and associated source document.
/// </summary>
public readonly struct Token(Symbol type, ReadOnlyMemory<char> text, int line, int column, SourceDocument document)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> struct with an invalid character.
    /// </summary>
    /// <param name="c">The invalid character.</param>
    /// <param name="line">The line number.</param>
    /// <param name="column">The column number.</param>
    /// <returns>A new instance of <see cref="Token"/> representing an invalid token.</returns>
    public static Token Invalid(char c, int line, int column, SourceDocument document) => new("#invalid", c.ToString().AsMemory(), line, column, document);

    /// <summary>
    /// Gets the symbol type of the token.
    /// </summary>
    public Symbol Type { get; } = type;

    /// <summary>
    /// Gets or sets the text of the token.
    /// </summary>
    public ReadOnlyMemory<char> Text { get; } = text;

    /// <summary>
    /// Gets the line number where the token occurs.
    /// </summary>
    public int Line { get; } = line;

    /// <summary>
    /// Gets the column number where the token starts.
    /// </summary>
    public int Column { get; } = column;

    /// <summary>
    /// Gets or sets the source document associated with the token.
    /// </summary>
    public SourceDocument Document { get; } = document;

    public Token(Symbol type, int line, int column, SourceDocument document)
    : this(type, "".AsMemory(), line, column, document)
    {
    }

    public override string ToString() => Text.ToString();

    /// <summary>
    /// Gets the starting <see cref="SourceSpan"/> of the current text segment.
    /// </summary>
    /// <returns>
    /// A <see cref="SourceSpan"/> representing the starting position of the current text segment, based on the current line and column.
    /// </returns>
    /// <remarks>
    /// This method creates a <see cref="SourceSpan"/> object that represents the starting position of the text segment.
    /// The starting position is defined by the current line and column properties.
    /// </remarks>
    public SourceSpan GetSourceSpanStart() => new(Line, Column);

    /// <summary>
    /// Gets the ending <see cref="SourceSpan"/> of the current text segment.
    /// </summary>
    /// <returns>
    /// A <see cref="SourceSpan"/> representing the ending position of the current text segment, calculated based on the current line, column, and the length of the text.
    /// </returns>
    /// <remarks>
    /// This method creates a <see cref="SourceSpan"/> object that represents the ending position of the text segment.
    /// The ending position is calculated as the current column plus the length of the text minus one, on the same line.
    /// </remarks>
    public SourceSpan GetSourceSpanEnd() => new(Line, Column + Text.Length - 1);

    /// <summary>
    /// Gets the <see cref="SourceRange"/> of the current text segment.
    /// </summary>
    /// <returns>
    /// A <see cref="SourceRange"/> object representing the range of the current text segment within the document.
    /// </returns>
    /// <remarks>
    /// This method creates a <see cref="SourceRange"/> object that represents the full range of the text segment within the document.
    /// It uses the starting and ending <see cref="SourceSpan"/> objects to define the range.
    /// </remarks>
    public SourceRange GetRange() => new(Document, GetSourceSpanStart(), GetSourceSpanEnd());

    public static bool operator ==(Token left, Token right) => left.Equals(right);
    public static bool operator ==(Token left, string right) => left.Type == right;
    public static bool operator !=(Token left, string right) => left.Type != right;

    public static bool operator !=(Token left, Token right) => !left.Equals(right);
    
    public bool Equals(Token other)
    {
        return Equals(Type, other.Type) && Text.Equals(other.Text) && Line == other.Line && Column == other.Column && Equals(Document, other.Document);
    }

    public override bool Equals(object obj)
    {
        return obj is Token other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Text, Line, Column, Document);
    }

    public Token Rewrite(Symbol type)
    {
        return new Token(type, type.Name.AsMemory(), Line, Column, Document);
    }
}
