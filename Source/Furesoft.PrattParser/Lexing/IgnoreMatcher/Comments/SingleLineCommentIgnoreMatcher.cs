namespace Furesoft.PrattParser.Lexing.IgnoreMatcher.Comments;

public class SingleLineCommentIgnoreMatcher : MultiLineCommentIgnoreMatcher
{
    public SingleLineCommentIgnoreMatcher(Symbol start) : base(start, PredefinedSymbols.EOL)
    {
    }
}
