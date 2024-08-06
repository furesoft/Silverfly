using System.Text.RegularExpressions;
using Silverfly.Text;

namespace Silverfly.Lexing.Matcher;

public class RegexMatcher(Symbol type, Regex regex): IMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(regex);
    }

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
