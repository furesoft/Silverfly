namespace Silverfly.Lexing.IgnoreMatcher.Comments;

/// <summary>
/// Matcher for ignoring multi-line comments in the lexer.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MultiLineCommentIgnoreMatcher"/> class.
/// </remarks>
/// <param name="start">The symbol representing the start of the multi-line comment.</param>
/// <param name="end">The symbol representing the end of the multi-line comment.</param>
public class MultiLineCommentIgnoreMatcher(Symbol start, Symbol end) : IIgnoreMatcher
{

    /// <summary>
    /// Determines if the current character should be ignored based on the start of the multi-line comment.
    /// </summary>
    /// <param name="lexer">The lexer instance.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>True if the character should be ignored; otherwise, false.</returns>
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(start);
    }

    /// <summary>
    /// Advances the lexer past the multi-line comment.
    /// </summary>
    /// <param name="lexer">The lexer instance.</param>
    public void Advance(Lexer lexer)
    {
        lexer.Advance(start.Name.Length);

        while (!lexer.IsMatch(end))
        {
            lexer.Advance();
        }

        lexer.Advance(end.Name.Length);
    }
}
