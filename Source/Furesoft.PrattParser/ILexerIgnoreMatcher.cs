namespace Furesoft.PrattParser;

public interface ILexerIgnoreMatcher
{
    bool Match(Lexer lexer, char c);
    void Advance(Lexer lexer);
}
