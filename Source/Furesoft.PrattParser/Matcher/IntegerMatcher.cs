namespace Furesoft.PrattParser.Matcher;

public class IntegerMatcher : ILexerPart
{
    public bool Match(Lexer lexer, char c)
    {
        return char.IsDigit(c);
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var document = lexer.Document;
        
        var oldColumn = column;
        var startIndex = index;

        while (index < document.Source.Length)
        {
            if (!char.IsDigit(document.Source[index]))
            {
                break;
            }

            column++;
            index++;
        }

        return new(PredefinedSymbols.Integer, document.Source.Substring(startIndex, index - startIndex), line,
            oldColumn);
    }
}
