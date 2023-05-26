namespace Furesoft.PrattParser.IgnoreMatcher.Comments;

public class SingleLineCommentIgnoreMatcher : MultiLineCommentIgnoreMatcher
{
    public SingleLineCommentIgnoreMatcher(Symbol start) : base(start, PredefinedSymbols.EOL)
    {
    }
}
