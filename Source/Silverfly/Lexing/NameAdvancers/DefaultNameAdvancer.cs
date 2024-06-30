namespace Silverfly.Lexing.NameAdvancers;

public class DefaultNameAdvancer : INameAdvancer
{
    public bool IsNameStart(char c)
    {
        return char.IsLetter(c);
    }

    public void AdvanceName(Lexer lexer)
    {
        while (lexer.IsNotAtEnd())
        {
            if (!char.IsLetter(lexer.Peek(0)))
            {
                break;
            }

            lexer.Advance();
        }
    }
}
