namespace Furesoft.PrattParser.IgnoreMatcher;

public class SingleLineCommentIgnoreMatcher : ILexerIgnoreMatcher
{
    private readonly Symbol _start;
    private readonly Symbol _end;

    public SingleLineCommentIgnoreMatcher(Symbol start, Symbol end)
    {
        _start = start;
        _end = end;
    }
    
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(_start);
    }

    public void Advance(Lexer lexer)
    {
        lexer.Advance(_start.Name.Length);

        while (!lexer.IsMatch(_end))
        {
            lexer.Advance();
        }
        
        lexer.Advance();
    }
}
