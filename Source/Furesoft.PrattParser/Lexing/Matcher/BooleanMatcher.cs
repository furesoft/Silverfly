namespace Furesoft.PrattParser.Lexing.Matcher;

public class BooleanMatcher : ILexerMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch("true") || lexer.IsMatch("false");
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        if (lexer.IsMatch("true"))
        {
            lexer.Advance("true".Length);
        }
        if (lexer.IsMatch("false"))
        {
            lexer.Advance("false".Length);
        }
        
        return new(PredefinedSymbols.Boolean, lexer.Document.Source.Slice(oldIndex, index - oldIndex), line, oldColumn);
    }
}
