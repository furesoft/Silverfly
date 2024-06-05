namespace Furesoft.PrattParser.Lexing;

public interface IIgnoreMatcher
{
    bool Match(Lexer lexer, char c);
    void Advance(Lexer lexer);
}
