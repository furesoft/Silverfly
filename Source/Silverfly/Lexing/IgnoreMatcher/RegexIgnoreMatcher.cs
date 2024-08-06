using System.Text.RegularExpressions;

namespace Silverfly.Lexing.IgnoreMatcher;

public class RegexIgnoreMatcher(Regex regex) : IIgnoreMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch(regex);
    }

    public void Advance(Lexer lexer)
    {
        var matches = regex.EnumerateMatches(lexer.Document.Source.Span[lexer.CurrentIndex..]);
        matches.MoveNext();
        
        lexer.Advance(matches.Current.Length);
    }
}
