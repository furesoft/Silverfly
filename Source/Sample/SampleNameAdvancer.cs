using Silverfly;
using Silverfly.Lexing;

namespace Sample;

public class SampleNameAdvancer : INameAdvancer
{
    public bool IsNameStart(char c)
    {
        return char.IsLetter(c) || c == '\'';
    }

    public void AdvanceName(Lexer lexer)
    {
        lexer.Advance();

        while (lexer.IsNotAtEnd())
        {
            if (!char.IsLetterOrDigit(lexer.Peek(0)) && !lexer.IsPunctuator(lexer.Peek(0).ToString()))
            {
                break;
            }

            lexer.Advance();
        }
    }
}