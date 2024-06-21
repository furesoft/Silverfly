
using System;

namespace Furesoft.PrattParser.Lexing.Matcher;

public class PredicateMatcher(Symbol symbol, Predicate<char> predicate) : IMatcher
{
    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        lexer.Advance();

        return new(symbol, lexer.Document.Source[oldIndex..index], line, oldColumn);
    }

    public bool Match(Lexer lexer, char c)
    {
        return predicate(c);
    }
}
