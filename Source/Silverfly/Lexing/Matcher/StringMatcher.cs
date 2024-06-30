namespace Silverfly.Lexing.Matcher;

using System;
using System.Text;
using Silverfly.Text;

public class StringMatcher(Symbol leftStr, Symbol rightStr, bool allowEscapeChars = true, bool allowUnicodeChars = true) : IMatcher
{
    private readonly Symbol _leftStr = leftStr;
    private readonly Symbol _rightStr = rightStr;

    public bool Match(Lexer lexer, char c)
    {
        return _leftStr != null && _rightStr != null && lexer.IsMatch(_leftStr);
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;

        lexer.Advance();

        var builder = new StringBuilder();
        while (!lexer.IsMatch(_rightStr.Name))
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
                builder.Append(lexer.Peek(0));
                lexer.Advance();
            }
        }

        lexer.Advance();

        return new Token(PredefinedSymbols.String, builder.ToString().AsMemory(), line, oldColumn);
    }

    private static void ParseEscapeChar(Lexer lexer, StringBuilder builder)
    {
        switch (lexer.Peek(0))
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
            case '"':
                builder.Append('"');
                break;
            default:
                builder.Append('\\');
                builder.Append(lexer.Peek(0));
                break;
        }

        lexer.Advance();
    }

    private static void ParseUnicodeEscape(Lexer lexer, StringBuilder builder, int line, int column)
    {
        var oldLine = line;
        var oldColumn = column;

        lexer.Advance(); // Consume 'u'

        int codePoint = 0;
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsDigit(lexer.Peek(0)) && !(lexer.IsBetween('a', 'f') || lexer.IsBetween('A', 'F')))
            {
                lexer.Document.Messages.Add(Message.Error("Invalid Unicode escape sequence", SourceRange.From(lexer.Document, oldLine, oldColumn, line, column)));
                return;
            }

            codePoint = codePoint * 16 + Convert.ToInt32(char.ToString(lexer.Peek(0)), 16);

            lexer.Advance();
        }

        builder.Append(char.ConvertFromUtf32(codePoint));
    }
}