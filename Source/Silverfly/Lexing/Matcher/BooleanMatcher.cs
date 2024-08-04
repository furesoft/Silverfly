using Silverfly.Text;

namespace Silverfly.Lexing.Matcher;

/// <summary>
/// Represents a matcher that identifies boolean literals ("true" or "false") in the lexer input.
/// </summary>
/// <param name="ignoreCasing">Determines whether the casing of the boolean literals should be ignored.</param>
public class BooleanMatcher(bool ignoreCasing = false) : IMatcher
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
        return lexer.IsMatch("true", ignoreCasing) || lexer.IsMatch("false", ignoreCasing);
    }

    /// <summary>
    /// Builds a token for the matched boolean literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <param name="document"></param>
    /// <returns>
    /// A <see cref="Token"/> representing the matched boolean literal.
    /// </returns>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line, SourceDocument document)
    {
        var oldColumn = column;
        var oldIndex = index;

        if (lexer.IsMatch("true", ignoreCasing))
        {
            lexer.Advance("true".Length);
        }
        else if (lexer.IsMatch("false", ignoreCasing))
        {
            lexer.Advance("false".Length);
        }

        return new(PredefinedSymbols.Boolean, lexer.Document.Source[oldIndex..index], line, oldColumn, document);
    }
}
