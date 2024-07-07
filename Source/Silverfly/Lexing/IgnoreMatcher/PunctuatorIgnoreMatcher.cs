namespace Silverfly.Lexing.IgnoreMatcher;

/// <summary>
/// Represents a matcher that ignores specific punctuator symbols in the lexer input.
/// </summary>
/// <param name="symbol">The punctuator symbol to be ignored.</param>
public class PunctuatorIgnoreMatcher(string symbol) : IIgnoreMatcher
{
    /// <summary>
    /// Determines whether the current lexer position matches the specified punctuator symbol.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>
    /// <c>true</c> if the current lexer position matches the punctuator symbol; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(symbol);
    }

    /// <summary>
    /// Advances the lexer's position by the length of the punctuator symbol.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    public void Advance(Lexer lexer)
    {
        lexer.Advance(symbol.Length);
    }
}
