namespace Furesoft.PrattParser.Lexing;


public interface INameAdvancer
{
    void AdvanceName(Lexer lexer);
    bool IsNameStart(char c);
}
