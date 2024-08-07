using System.Text.RegularExpressions;
using Silverfly.Text;

namespace Silverfly.Lexing.Matcher;

/// <summary>
/// Represents a matcher that uses a regular expression to match and build tokens of a specified type.
/// </summary>
public class RegexMatcher(Symbol type, Regex regex): IMatcher
{
    /// <summary>
    /// Determines whether the specified characters matches the pattern.
    /// </summary>
    /// <param name="lexer">The lexer providing the input text.</param>
    /// <param name="c">The character to match.</param>
    /// <returns><c>true</c> if the character matches the pattern; otherwise, <c>false</c>.</returns>
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(regex);
    }

    /// <summary>
    /// Builds a token from the matched pattern and advances the lexer.
    /// </summary>
    /// <param name="lexer">The lexer providing the input text.</param>
    /// <param name="index">The current index in the input text, passed by reference.</param>
    /// <param name="column">The current column in the input text, passed by reference.</param>
    /// <param name="line">The current line in the input text, passed by reference.</param>
    /// <returns>A <see cref="Token"/> representing the matched pattern.</returns>
    /// <remarks>
    /// If no match is found, an error message is added to the lexer's document.
    /// The lexer is advanced by the length of the matched pattern.
    /// </remarks>
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;
        var oldLine = line;

        var matches = regex.EnumerateMatches(lexer.Document.Source.Span, index);

        matches.MoveNext();
        if (matches.Current.Length == 0)
        {
            lexer.Document.AddMessage(MessageSeverity.Error, "Cannot match regex pattern", oldLine, oldColumn, line, column);
        }
        
        lexer.Advance(matches.Current.Length);

        return new(type, lexer.Document.Source[oldIndex..index], line, oldColumn, lexer.Document);
    }
}
