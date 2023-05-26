namespace Furesoft.PrattParser.Lexing.IgnoreMatcher;

public class PunctuatorIgnoreMatcher : ILexerIgnoreMatcher
{
    private readonly string _symbol;

    public PunctuatorIgnoreMatcher(string symbol)
    {
        _symbol = symbol;
    }

    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(_symbol);
    }

    public void Advance(Lexer lexer)
    {
        lexer.Advance(_symbol.Length);
    }
}
