namespace Furesoft.PrattParser.Lexing.NameAdvancers;

public class CStyleNameAdvancer : INameAdvancer
{
    public bool IsNameStart(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    public void AdvanceName(Lexer lexer)
    {
        while (lexer.IsNotAtEnd())
        {
            if (!char.IsLetterOrDigit(lexer.Peek(0)))
            {
                break;
            }

            lexer.Advance();
        }
    }
}
