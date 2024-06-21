namespace Furesoft.PrattParser;


public interface INameAdvancer
{
    void AdvanceName(Lexer lexer);
    bool IsNameStart(char c);
}
