namespace Silverfly.Lexing.NameAdvancers;

/// <summary>
/// Represents a name advancer that identifies and processes names starting with a letter.
/// </summary>
public class DefaultNameAdvancer : INameAdvancer
{
    /// <summary>
    /// Determines whether the specified character is a valid starting character for a name.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>
    /// <c>true</c> if the specified character is a letter; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNameStart(char c)
    {
        return char.IsLetter(c);
    }

    /// <summary>
    /// Advances the lexer's position over a name that starts with a letter.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    public void AdvanceName(Lexer lexer)
    {
        while (lexer.IsNotAtEnd())
        {
            if (!char.IsLetter(lexer.Peek()))
            {
                break;
            }

            lexer.Advance();
        }
    }
}
