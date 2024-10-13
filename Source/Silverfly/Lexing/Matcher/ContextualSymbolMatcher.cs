namespace Silverfly.Lexing.Matcher;

public class ContextualSymbolMatcher<TContext>(Symbol start) : IMatcher
    where TContext : ILexerContext, new()
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsContext<TContext>() && lexer.IsMatch(start);
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        lexer.Advance(start.Name.Length);
        return new(start, lexer.Document.Source[oldIndex..index], line, oldColumn, lexer.Document);
    }
}
