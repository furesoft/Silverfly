using System;

namespace Silverfly.Lexing.IgnoreMatcher;
/// <summary>
/// Represents a matcher that ignores characters based on a specified predicate.
/// </summary>
/// <param name="predicate">The predicate used to determine which characters to ignore.</param>
public class PredicateIgnoreMatcher(Predicate<char> predicate) : IIgnoreMatcher
{
    /// <summary>
    /// Determines whether the specified character matches the predicate and should be ignored.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The character to check against the predicate.</param>
    /// <returns>
    /// <c>true</c> if the character matches the predicate and should be ignored; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        return predicate(c);
    }

    /// <summary>
    /// Advances the lexer's position by one character.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    public void Advance(Lexer lexer)
    {
        lexer.Advance();
    }
}
