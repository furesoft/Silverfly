namespace Furesoft.PrattParser.Lexing.IgnoreMatcher;

public class PunctuatorIgnoreMatcher(string symbol) : ILexerIgnoreMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(symbol);
    }

    public void Advance(Lexer lexer)
    {
        lexer.Advance(symbol.Length);
    }
}
