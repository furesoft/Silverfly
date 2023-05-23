namespace Furesoft.PrattParser.Matcher;

public class SignedIntegerMatcher : ILexerPart
{
    public bool Match(Lexer lexer, char c)
    {
        return c == '-' && char.IsDigit(lexer.Peek(1));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        do
        {
            index++;
            column++;
        } while (index < lexer.Document.Source.Length && char.IsDigit(lexer.Document.Source[index]));

        return new(PredefinedSymbols.SignedInteger, lexer.Document.Source.Substring(oldIndex, index - oldIndex),
            line, column);
    }
}
