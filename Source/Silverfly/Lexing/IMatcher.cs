namespace Silverfly.Lexing;

/// <summary>
/// Defines methods for matching and building tokens based on lexer input.
/// </summary>
public interface IMatcher
{
    /// <summary>
    /// Determines whether the matcher can match the current character in the lexer input.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character to match.</param>
    /// <returns>
    /// <c>true</c> if the matcher can match the current character; otherwise, <c>false</c>.
    /// </returns>
    bool Match(Lexer lexer, char c);

    /// <summary>
    /// Builds a token based on the current lexer state.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the input source.</param>
    /// <param name="column">The current column in the input source.</param>
    /// <param name="line">The current line in the input source.</param>
    /// <returns>The constructed token.</returns>
    Token Build(Lexer lexer, ref int index, ref int column, ref int line);
}
