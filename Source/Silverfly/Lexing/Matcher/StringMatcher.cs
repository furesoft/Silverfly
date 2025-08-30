using System;
using System.Text;
using System.Xml.Schema;
using Silverfly.Text;

namespace Silverfly.Lexing.Matcher;

/// <summary>
///     Represents a matcher that identifies and parses string tokens.
/// </summary>
/// <param name="leftStr">The symbol representing the start of a string.</param>
/// <param name="rightStr">The symbol representing the end of a string.</param>
/// <param name="allowEscapeChars">Indicates whether escape characters are allowed within the string.</param>
/// <param name="allowUnicodeChars">Indicates whether Unicode escape sequences are allowed within the string.</param>
public class StringMatcher(Symbol leftStr, Symbol rightStr, bool allowEscapeChars = true, bool allowUnicodeChars = true)
    : IMatcher
{
    public Symbol Left { get; set; } = leftStr;
    public Symbol Right { get; set; } = rightStr;

    /// <summary>
    ///     Determines whether the current lexer position matches the start of a string.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>
    ///     <c>true</c> if the current lexer position matches the start of a string; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        return Left != null && Right != null && lexer.IsMatch(Left);
    }

    /// <summary>
    ///     Builds a token for the matched string input.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <param name="document"></param>
    /// <returns>
    ///     A <see cref="Token" /> representing the matched string input.
    /// </returns>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;

        var startSpan = new SourceSpan(line, column);
        lexer.Advance();

        //StringBuilder is mandatory to build an escaped string
        var builder = new StringBuilder();

        while (lexer.IsNotAtEnd() && !lexer.IsMatch(Right.Name))
        {
            if (lexer.IsMatch("\\") && allowEscapeChars)
            {
                lexer.Advance();

                if (lexer.IsMatch("u") && allowUnicodeChars)
                {
                    ParseUnicodeEscape(lexer, builder, line, column);
                }
                else
                {
                    ParseEscapeChar(lexer, builder);
                }
            }
            else
            {
                builder.Append(lexer.Peek());
                lexer.Advance();
            }
        }

        if (!lexer.IsMatch(Right))
        {
            var endSpan = new SourceSpan(line, column);
            lexer.Advance();

            lexer.Document.AddMessage(MessageSeverity.Error,
                "String is not closed", new SourceRange(lexer.Document, startSpan, endSpan));

            return Token.Invalid('\0', line, column, lexer.Document);
        }

        lexer.Advance();

        return new Token(PredefinedSymbols.String, builder.ToString().AsMemory(), line, oldColumn, lexer.Document);
    }

    /// <summary>
    ///     Parses an escape character within a string and appends it to the string builder.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="builder">The string builder to append the escape character to.</param>
    private void ParseEscapeChar(Lexer lexer, StringBuilder builder)
    {
        switch (lexer.Peek())
        {
            case '\\':
                builder.Append('\\');
                break;
            case 'n':
                builder.Append('\n');
                break;
            case 'r':
                builder.Append('\r');
                break;
            case 't':
                builder.Append('\t');
                break;
            case 'f':
                builder.Append('\f');
                break;
            default:
                if (lexer.IsMatch(leftStr))
                {
                    builder.Append(leftStr);
                    break;
                }

                builder.Append('\\');
                builder.Append(lexer.Peek());
                lexer.Document.AddMessage(MessageSeverity.Error, "Invalid escape character",
                    SourceRange.From(lexer.Document, lexer.Line, lexer.Column, lexer.Line, lexer.Column));
                break;
        }

        lexer.Advance();
    }

    /// <summary>
    ///     Parses a Unicode escape sequence within a string and appends it to the string builder.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="builder">The string builder to append the Unicode character to.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    private static void ParseUnicodeEscape(Lexer lexer, StringBuilder builder, int line, int column)
    {
        var oldLine = line;
        var oldColumn = column;

        lexer.Advance(); // Consume 'u'

        var codePoint = 0;
        for (var i = 0; i < 4; i++)
        {
            if (!char.IsDigit(lexer.Peek()) && !(lexer.IsBetween('a', 'f') || lexer.IsBetween('A', 'F')))
            {
                lexer.Document.Messages.Add(Message.Error("Invalid Unicode escape sequence",
                    SourceRange.From(lexer.Document, oldLine, oldColumn, line, column)));
                return;
            }

            codePoint = (codePoint * 16) + Convert.ToInt32(char.ToString(lexer.Peek()), 16);

            lexer.Advance();
        }

        builder.Append(char.ConvertFromUtf32(codePoint));
    }
}
