namespace Furesoft.PrattParser.IgnoreMatcher;

public class SingleLineCommentIgnoreMatcher : MultiLineCommentIgnoreMatcher
{
    public SingleLineCommentIgnoreMatcher(Symbol start) : base(start, PredefinedSymbols.EOL)
    {
    }
}
