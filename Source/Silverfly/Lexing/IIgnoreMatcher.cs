namespace Silverfly.Lexing;

/// <summary>
///     Defines methods for matching and advancing over ignored characters or symbols in a lexer.
/// </summary>
public interface IIgnoreMatcher
{
    /// <summary>
    ///     Determines whether the specified character matches the criteria for ignoring.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The character to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified character should be ignored; otherwise, <c>false</c>.
    /// </returns>
    bool Match(Lexer lexer, char c);

    /// <summary>
    ///     Advances the lexer's position over the current ignored character or symbol.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    void Advance(Lexer lexer);
}
