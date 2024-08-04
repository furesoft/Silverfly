using System;
using Silverfly.Text;

namespace Silverfly.Lexing.Matcher;

/// <summary>
/// Represents a matcher that identifies enum values in the lexer input.
/// </summary>
/// <typeparam name="T">The enum type to match against.</typeparam>
/// <param name="symbol">The symbol representing the matched enum value.</param>
public class EnumMatcher<T>(string symbol) : IMatcher
    where T : struct
{
    /// <summary>
    /// Determines whether the current lexer position matches an enum value.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="c">The current character being processed.</param>
    /// <returns>
    /// <c>true</c> if the current lexer position matches an enum value; otherwise, <c>false</c>.
    /// </returns>
    public bool Match(Lexer lexer, char c)
    {
        foreach (var name in Enum.GetNames(typeof(T)))
        {
            if (lexer.IsMatch(name.ToLower()))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Builds a token for the matched enum value.
    /// </summary>
    /// <param name="lexer">The lexer processing the input.</param>
    /// <param name="index">The current index in the lexer's input source.</param>
    /// <param name="column">The current column in the lexer's input source.</param>
    /// <param name="line">The current line in the lexer's input source.</param>
    /// <param name="document"></param>
    /// <returns>
    /// A <see cref="Token"/> representing the matched enum value.
    /// </returns>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line, SourceDocument document)
    {
        var oldColumn = column;
        var oldIndex = index;

        foreach (var name in Enum.GetNames(typeof(T)))
        {
            if (lexer.IsMatch(name.ToLower()))
            {
                lexer.Advance(name.Length);
                break;
            }
        }

        return new(symbol, lexer.Document.Source[oldIndex..index], line, oldColumn, document);
    }
}
