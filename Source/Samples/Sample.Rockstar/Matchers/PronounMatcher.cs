using Silverfly;
using Silverfly.Lexing;

namespace Sample.Rockstar.Matchers;

public class PronounMatcher : IMatcher
{
    public static readonly string[] Pronouns = ["it", "he", "she", "him", "her", "they", "them", "ze", "hir", "zie", "zir", "xe", "xem", "ve", "ver"];
    public bool Match(Lexer lexer, char c)
    {
        return Pronouns.Any(alias => lexer.IsMatch(alias));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        foreach (var alias in Pronouns)
        {
            if (lexer.IsMatch(alias))
            {
                lexer.Advance(alias.Length);
            }
        }

        return new("#pronoun", lexer.Document.Source[oldIndex..index], line, oldColumn, lexer.Document);
    }
}
