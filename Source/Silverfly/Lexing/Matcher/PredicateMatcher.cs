
using System;

namespace Silverfly.Lexing.Matcher;

/// <summary>
/// Represents a matcher that identifies and parses tokens based on a provided predicate.
/// </summary>
/// <param name="symbol">The symbol associated with the matched token.</param>
/// <param name="predicate">The predicate used to determine if a character is part of the token.</param>
public class PredicateMatcher(Symbol symbol, Predicate<char> predicate) : IMatcher
{
    /// <summary>
    /// Builds a token for the matched input based on the provided predicate.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <returns>
    /// A <see cref="Token"/> representing the matched input.
    /// </returns>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        lexer.Advance();

        return new(symbol, lexer.Document.Source[oldIndex..index], line, oldColumn);
    }

    /// <summary>
    /// Determines whether the current lexer position matches the criteria defined by the predicate.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>
    /// <c>true</c> if the current lexer position matches the criteria defined by the predicate; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        return predicate(c);
    }
}
