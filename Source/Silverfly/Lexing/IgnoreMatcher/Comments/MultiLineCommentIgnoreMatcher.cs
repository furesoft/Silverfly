namespace Silverfly.Lexing.IgnoreMatcher.Comments;

public class MultiLineCommentIgnoreMatcher(Symbol start, Symbol end) : IIgnoreMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(start);
    }

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
