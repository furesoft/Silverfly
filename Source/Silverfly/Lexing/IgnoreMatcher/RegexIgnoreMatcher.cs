using System.Text.RegularExpressions;

namespace Silverfly.Lexing.IgnoreMatcher;

/// <summary>
/// Represents a matcher that ignores text based on a regular expression.
/// </summary>
public class RegexIgnoreMatcher(Regex regex) : IIgnoreMatcher
{
    /// <summary>
    /// Determines whether the specified character matches the ignore pattern.
    /// </summary>
    /// <param name="lexer">The lexer providing the input text.</param>
    /// <param name="c">The character to match.</param>
    /// <returns><c>true</c> if the character matches the ignore pattern; otherwise, <c>false</c>.</returns>
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(regex);
    }

    /// <summary>
    /// Advances the lexer past the matched ignore pattern.
    /// </summary>
    /// <param name="lexer">The lexer to advance.</param>
    public void Advance(Lexer lexer)
    {
        var matches = regex.EnumerateMatches(lexer.Document.Source.Span[lexer.CurrentIndex..]);
        matches.MoveNext();
        
        lexer.Advance(matches.Current.Length);
    }
}
