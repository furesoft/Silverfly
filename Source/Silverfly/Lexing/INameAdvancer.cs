namespace Silverfly.Lexing;

/// <summary>
///     Defines methods for advancing through and validating names in a lexer.
/// </summary>
public interface INameAdvancer
{
    /// <summary>
    ///     Advances the lexer through the current name.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    void AdvanceName(Lexer lexer);

    /// <summary>
    ///     Determines whether the specified character can start a name.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>
    ///     <c>true</c> if the character can start a name; otherwise, <c>false</c>.
    /// </returns>
    bool IsNameStart(char c);
}
