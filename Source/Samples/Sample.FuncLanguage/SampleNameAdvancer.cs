using Silverfly.Lexing;

namespace Silverfly.Sample.Func;

public class SampleNameAdvancer : INameAdvancer
{
    private static readonly List<char> _operators =
    [
        '+', '-', '*', '/'
    ];

    public bool IsNameStart(char c)
    {
        return char.IsLetter(c) || c == '\'';
    }

    public void AdvanceName(Lexer lexer)
    {
        lexer.Advance();

        while (lexer.IsNotAtEnd())
        {
            if (IsOperator(lexer.Peek()))
            {
                lexer.Advance();
                return;
            }

            if (!char.IsLetterOrDigit(lexer.Peek()) && lexer.Peek() != '_')
            {
                break;
            }

            lexer.Advance();
        }
    }

    private static bool IsOperator(char c)
    {
        return _operators.Contains(c);
    }
}
