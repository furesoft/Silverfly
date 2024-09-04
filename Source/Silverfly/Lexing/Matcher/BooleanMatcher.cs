namespace Silverfly.Lexing.Matcher;

/// <summary>
/// Represents a matcher that identifies boolean literals ("true" or "false") in the lexer input.
/// </summary>
public class BooleanMatcher : IMatcher
{
    /// <summary>
    /// Determines whether the current lexer position matches a boolean literal ("true" or "false").
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>
    /// <c>true</c> if the current lexer position matches a boolean literal; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch("true", lexer.Config.IgnoreCasing) || lexer.IsMatch("false", lexer.Config.IgnoreCasing);
    }

    /// <summary>
    /// Builds a token for the matched boolean literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <returns>
    /// A <see cref="Token"/> representing the matched boolean literal.
    /// </returns>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        lexer.AdvanceIfMatch("true");
        lexer.AdvanceIfMatch("false");

        return new(PredefinedSymbols.Boolean, lexer.Document.Source[oldIndex..index], line, oldColumn, lexer.Document);
    }
}
