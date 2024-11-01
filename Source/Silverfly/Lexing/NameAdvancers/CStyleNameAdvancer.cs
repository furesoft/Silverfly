namespace Silverfly.Lexing.NameAdvancers;

/// <summary>
/// Represents a name advancer that identifies and processes names following the C-style naming conventions.
/// </summary>
public class CStyleNameAdvancer : INameAdvancer
{
    /// <summary>
    /// Determines whether the specified character is a valid starting character for a C-style name.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>
    /// <c>true</c> if the specified character is a letter or an underscore; otherwise, <c>false</c>.
    /// </returns>
    public bool IsNameStart(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    /// <summary>
    /// Advances the lexer's position over a C-style name.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    public void AdvanceName(Lexer lexer)
    {
        while (lexer.IsNotAtEnd())
        {
            if (!IsNameStart(lexer.Peek()) && !char.IsDigit(lexer.Peek()))
            {
                break;
            }

            lexer.Advance();
        }
    }
}
