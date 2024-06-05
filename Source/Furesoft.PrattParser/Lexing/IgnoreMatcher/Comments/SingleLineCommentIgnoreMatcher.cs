namespace Furesoft.PrattParser.Lexing.IgnoreMatcher.Comments;

public class SingleLineCommentIgnoreMatcher(Symbol start) : MultiLineCommentIgnoreMatcher(start, PredefinedSymbols.EOL)
{
}
