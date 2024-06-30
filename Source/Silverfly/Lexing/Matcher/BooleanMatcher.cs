namespace Silverfly.Lexing.Matcher;

public class BooleanMatcher(bool ignoreCasing = false) : IMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch("true", ignoreCasing) || lexer.IsMatch("false", ignoreCasing);
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        if (lexer.IsMatch("true", ignoreCasing))
        {
            lexer.Advance("true".Length);
        }
        else if (lexer.IsMatch("false", ignoreCasing))
        {
            lexer.Advance("false".Length);
        }

        return new(PredefinedSymbols.Boolean, lexer.Document.Source[oldIndex..index], line, oldColumn);
    }
}
