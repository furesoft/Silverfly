namespace Furesoft.PrattParser.Lexing;

public interface INameMatcher
{
    bool Match(Lexer lexer, char c);
}
