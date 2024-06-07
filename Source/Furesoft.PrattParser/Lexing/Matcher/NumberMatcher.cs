using System;

namespace Furesoft.PrattParser.Lexing.Matcher;

public class NumberMatcher(bool allowHex, bool allowBin, Symbol floatingPointSymbol, Symbol seperatorSymbol = null) : IMatcher
{

    public bool Match(Lexer lexer, char c)
    {
        var isnegative = c == '-' && char.IsDigit(lexer.Peek(1));
        var isDigit = char.IsDigit(lexer.Peek(0));
        var isHexDigit = lexer.IsMatch("0x");
        var isBinaryDigit = lexer.IsMatch("0b");

        return (isHexDigit && allowHex) || (isBinaryDigit && allowBin) || isnegative || isDigit;
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        AdvanceNumber(lexer, ref index);
        AdvanceFloatingPointNumber(lexer, ref index);

        var textWithoutSeperator = lexer.Document.Source[oldIndex..index].ToString()
            .Replace(seperatorSymbol.Name, "");

        return new Token(PredefinedSymbols.Number, textWithoutSeperator.AsMemory(), line, oldColumn);
    }

    private void AdvanceNumber(Lexer lexer, ref int index)
    {
        if (lexer.IsMatch("0x"))
        {
            AdvanceHexNumber(lexer, ref index);
            return;
        }

        if (lexer.IsMatch("0b"))
        {
            AdvanceBinNumber(lexer, ref index);
            return;
        }

        AdvanceNumber(lexer, ref index, char.IsDigit);
    }

    private void AdvanceBinNumber(Lexer lexer, ref int index)
    {
        AdvanceNumber(lexer, ref index, IsValidBinChar, 2);
    }

    private void AdvanceHexNumber(Lexer lexer, ref int index)
    {
        AdvanceNumber(lexer, ref index, IsValidHexChar, 2);
    }

    private bool IsValidBinChar(char c) => c is '1' or '0';

    private void AdvanceFloatingPointNumber(Lexer lexer, ref int index)
    {
        if (lexer.IsMatch(floatingPointSymbol.Name))
        {
            if (!char.IsDigit(lexer.Peek(1)))
            {
                return;
            }

            lexer.Advance();
            AdvanceNumber(lexer, ref index, char.IsDigit);

            // Handle E-Notation
            if (lexer.Peek(0) == 'e' || lexer.Peek(0) == 'E')
            {
                AdvanceNumber(lexer, ref index, char.IsDigit, 1);
            }
        }
    }

    private void AdvanceNumber(Lexer lexer, ref int index, Predicate<char> charPredicate, int preskip = 0)
    {
        lexer.Advance(preskip);

        do
        {
            lexer.Advance();
        } while (index < lexer.Document.Source.Length && charPredicate(lexer.Peek(0)) || lexer.IsMatch(seperatorSymbol.Name));
    }

    private bool IsValidHexChar(char c) => char.IsDigit(c) || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
}