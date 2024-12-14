using Silverfly.Lexing;

namespace Silverfly.Sample.Rockstar.Matchers;

public class AliasedBooleanMatcher : IMatcher
{
    public static readonly string[] TrueAliases = ["true", "right", "yes", "ok"];
    public static readonly string[] FalseAliases = ["false", "wrong", "no", "lies"];

    public bool Match(Lexer lexer, char c)
    {
        return TrueAliases.Concat(FalseAliases).Any(alias => lexer.IsMatch(alias));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        foreach (var alias in TrueAliases.Concat(FalseAliases))
        {
            if (lexer.IsMatch(alias))
            {
                lexer.Advance(alias.Length);
                break;
            }
        }

        return new Token(PredefinedSymbols.Boolean, lexer.Document.Source[oldIndex..index], line, oldColumn,
            lexer.Document);
    }
}
