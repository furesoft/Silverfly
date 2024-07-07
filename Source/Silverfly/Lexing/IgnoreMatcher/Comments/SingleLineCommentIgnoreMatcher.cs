namespace Silverfly.Lexing.IgnoreMatcher.Comments;

/// <summary>
/// Matcher for ignoring single-line comments in the lexer.
/// </summary>
public class SingleLineCommentIgnoreMatcher : MultiLineCommentIgnoreMatcher
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleLineCommentIgnoreMatcher"/> class.
    /// </summary>
    /// <param name="start">The symbol representing the start of the single-line comment.</param>
    public SingleLineCommentIgnoreMatcher(Symbol start)
        : base(start, PredefinedSymbols.EOL)
    {
    }
}
