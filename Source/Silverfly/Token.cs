using System;
using Silverfly.Text;

namespace Silverfly;

/// <summary>
/// Represents a token in the lexer, which encapsulates a symbol type, text, line, column, and associated source document.
/// </summary>
public struct Token(Symbol type, ReadOnlyMemory<char> text, int line, int column)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> struct with an invalid character.
    /// </summary>
    /// <param name="c">The invalid character.</param>
    /// <param name="line">The line number.</param>
    /// <param name="column">The column number.</param>
    /// <returns>A new instance of <see cref="Token"/> representing an invalid token.</returns>
    public static Token Invalid(char c, int line, int column) => new("#invalid", c.ToString().AsMemory(), line, column);

    /// <summary>
    /// Gets the symbol type of the token.
    /// </summary>
    public Symbol Type { get; } = type;

    /// <summary>
    /// Gets or sets the text of the token.
    /// </summary>
    public ReadOnlyMemory<char> Text { get; set; } = text;

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
    public SourceDocument Document { get; set; }

    public Token(Symbol type, int line, int column)
    : this(type, "".AsMemory(), line, column)
    {
    }

    public override readonly string ToString() => Text.ToString();

    public readonly SourceSpan GetSourceSpanStart() => new(Line, Column);

    public readonly SourceSpan GetSourceSpanEnd() => new(Line, Column + Text.Length - 1);

    public Token WithDocument(SourceDocument document)
    {
        Document = document;

        return this;
    }

    public readonly SourceRange GetRange() => new(Document, GetSourceSpanStart(), GetSourceSpanEnd());

    public static bool operator ==(Token left, Token right) => left.Equals(right);

    public static bool operator !=(Token left, Token right) => !left.Equals(right);

    public override readonly bool Equals(object obj) => base.Equals(obj);

    public override readonly int GetHashCode() => base.GetHashCode();
}
