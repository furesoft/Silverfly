using System;

namespace Silverfly.Lexing.Matcher;

/// <summary>
/// Represents a matcher that identifies and parses number literals in the lexer input.
/// </summary>
/// <param name="allowHex">If set to <c>true</c>, allows hexadecimal numbers.</param>
/// <param name="allowBin">If set to <c>true</c>, allows binary numbers.</param>
/// <param name="floatingPointSymbol">The symbol representing a floating-point number.</param>
/// <param name="seperatorSymbol">The symbol used as a separator in the number (e.g., '_').</param>
public class NumberMatcher(bool allowHex, bool allowBin, Symbol floatingPointSymbol, Symbol seperatorSymbol = null) : IMatcher
{
    /// <summary>
    /// Determines whether the current lexer position matches a number literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>
    /// <c>true</c> if the current lexer position matches a number literal; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        var isDigit = char.IsDigit(lexer.Peek(0));
        var isHexDigit = lexer.IsMatch("0x");
        var isBinaryDigit = lexer.IsMatch("0b");

        return (isHexDigit && allowHex) || (isBinaryDigit && allowBin) || isDigit;
    }

    /// <summary>
    /// Builds a token for the matched number literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <param name="document"></param>
    /// <returns>
    /// A <see cref="Token"/> representing the matched number literal.
    /// </returns>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        AdvanceNumber(lexer, ref index);
        AdvanceFloatingPointNumber(lexer, ref index);

        var textWithoutSeperator = lexer.Document.Source[oldIndex..index].ToString()
            .Replace(seperatorSymbol.Name, "");

        return new Token(PredefinedSymbols.Number, textWithoutSeperator.AsMemory(), line, oldColumn, lexer.Document);
    }

    /// <summary>
    /// Advances the lexer through a number literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
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

    /// <summary>
    /// Advances the lexer through a binary number literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    private void AdvanceBinNumber(Lexer lexer, ref int index)
    {
        AdvanceNumber(lexer, ref index, IsValidBinChar, 2);
    }

    /// <summary>
    /// Advances the lexer through a hexadecimal number literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    private void AdvanceHexNumber(Lexer lexer, ref int index)
    {
        AdvanceNumber(lexer, ref index, IsValidHexChar, 2);
    }

    /// <summary>
    /// Advances the lexer through a floating-point number literal.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
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

    /// <summary>
    /// Advances the lexer through a number literal, based on a predicate.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="charPredicate">The predicate that determines valid characters in the number literal.</param>
    /// <param name="preskip">The number of characters to skip before starting the advancement.</param>
    private void AdvanceNumber(Lexer lexer, ref int index, Predicate<char> charPredicate, int preskip = 0)
    {
        lexer.Advance(preskip);

        do
        {
            lexer.Advance();
        } while ((index < lexer.Document.Source.Length && charPredicate(lexer.Peek(0))) ||
                 lexer.IsMatch(seperatorSymbol.Name));
    }

    /// <summary>
    /// Determines whether a character is a valid binary digit (0 or 1).
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns><c>true</c> if the character is a valid binary digit; otherwise, <c>false</c>.</returns>
    private bool IsValidBinChar(char c) => c is '1' or '0';

    /// <summary>
    /// Determines whether a character is a valid hexadecimal digit.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns><c>true</c> if the character is a valid hexadecimal digit; otherwise, <c>false</c>.</returns>
    private bool IsValidHexChar(char c) => char.IsDigit(c) || c is >= 'a' and <= 'z' || c is >= 'A' and <= 'Z';
}
