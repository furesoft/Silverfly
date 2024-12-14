using Silverfly.Lexing;

namespace Silverfly.Sample.Rockstar;

public class RockstarNameAdvancer : INameAdvancer
{
    private static readonly string[] CommonVariableStarts = ["a", "an", "the", "my", "your", "our"];

    public bool IsNameStart(char c)
    {
        return char.IsLetter(c);
    }

    public void AdvanceName(Lexer lexer)
    {
        // Check if we're at the start of a proper variable
        if (char.IsUpper(lexer.Peek()))
        {
            while (lexer.IsNotAtEnd())
            {
                // Read the first part of the proper noun
                ReadWhile(lexer, char.IsLetter);

                SkipWhitespace(lexer);

                // Check if the next word also starts with an uppercase letter
                if (!char.IsUpper(lexer.Peek()))
                {
                    break; // Not a proper noun part, stop advancing
                }
            }
        }
        else if (IsCommonVariableStart(lexer)) // Check if we're at the start of a common variable
        {
            foreach (var alias in CommonVariableStarts)
            {
                if (lexer.IsMatch(alias))
                {
                    lexer.Advance(alias.Length);
                    break;
                }
            }

            SkipWhitespace(lexer);

            ReadWhile(lexer, char.IsLower); // Read the lowercase variable name
        }
        else if (IsSimpleVariableStart(lexer)) // Check for a simple variable
        {
            ReadWhile(lexer, char.IsLetter); // Read the entire simple variable name
        }
    }

    private static bool IsCommonVariableStart(Lexer lexer)
    {
        foreach (var keyword in CommonVariableStarts)
        {
            if (lexer.IsMatch(keyword, true))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSimpleVariableStart(Lexer lexer)
    {
        return char.IsLetter(lexer.Peek());
    }

    private static void SkipWhitespace(Lexer lexer)
    {
        while (lexer.IsNotAtEnd() && char.IsWhiteSpace(lexer.Peek()))
        {
            lexer.Advance();
        }
    }

    private static void ReadWhile(Lexer lexer, Func<char, bool> condition)
    {
        while (lexer.IsNotAtEnd() && condition(lexer.Peek()))
        {
            lexer.Advance();
        }
    }
}
