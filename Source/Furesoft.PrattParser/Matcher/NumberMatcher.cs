using System;

namespace Furesoft.PrattParser.Matcher;

public class NumberMatcher : ILexerMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        var isnegative = c == '-' && char.IsDigit(lexer.Peek(1));
        var isDigit = char.IsDigit(lexer.Peek(0));
        var isHexDigit = lexer.IsMatch("0x");
        var isBinaryDigit = lexer.IsMatch("0b");

        return isHexDigit || isBinaryDigit || isnegative || isDigit;
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        if (lexer.IsMatch("0x"))
        {
            LexHexNumber(lexer, ref index);
            goto createToken;
        }

        if (lexer.IsMatch("0b"))
        {
            LexBinNumber(lexer, ref index);
            goto createToken;
        }

        AdvanceNumber(lexer, ref index, char.IsDigit);

        LexFloatingPointNumber(lexer, ref index, ref column);

        createToken:
        return new(PredefinedSymbols.Number, lexer.Document.Source.Substring(oldIndex, index - oldIndex),
            line, oldColumn);
    }

    private void LexBinNumber(Lexer lexer, ref int index)
    {
        lexer.Advance();
        lexer.Advance();
        
        AdvanceNumber(lexer, ref index, IsValidBinChar);
    }

    private void LexHexNumber(Lexer lexer, ref int index)
    {
        lexer.Advance();
        lexer.Advance();
        
        AdvanceNumber(lexer, ref index, IsValidHexChar);
    }
    
    private bool IsValidBinChar(char c)
    {
        return c is '1' or '0';
    }


    private static void LexFloatingPointNumber(Lexer lexer, ref int index, ref int column)
    {
        if (lexer.Peek(0) == '.')
        {
            lexer.Advance();

            AdvanceNumber(lexer, ref index, char.IsDigit);

            // Handle E-Notation
            if (lexer.Peek(0) == 'e' || lexer.Peek(0) == 'E')
            {
                lexer.Advance();

                AdvanceNumber(lexer, ref index, char.IsDigit);
            }
        }
    }

    private static void AdvanceNumber(Lexer lexer, ref int index, Predicate<char> charPredicate)
    {
        do
        {
            lexer.Advance();
        } while (index < lexer.Document.Source.Length && charPredicate(lexer.Document.Source[index]));
    }

    private bool IsValidHexChar(char c) => char.IsDigit(c) || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
}
