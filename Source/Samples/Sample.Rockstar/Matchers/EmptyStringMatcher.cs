using Silverfly;
using Silverfly.Lexing;

namespace Sample.Rockstar.Matchers;

public class EmptyStringMatcher : IMatcher
{
    public static readonly string[] Aliases = ["empty", "silent", "silence"];
    public bool Match(Lexer lexer, char c)
    {
        return Aliases.Any(alias => lexer.IsMatch(alias));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        foreach (var alias in Aliases)
        {
            if (lexer.IsMatch(alias))
            {
                lexer.Advance(alias.Length);
            }
        }

        return new("#empty_string", lexer.Document.Source[oldIndex..index], line, oldColumn, lexer.Document);
    }
}
