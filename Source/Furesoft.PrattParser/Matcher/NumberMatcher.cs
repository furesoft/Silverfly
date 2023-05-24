namespace Furesoft.PrattParser.Matcher;

public class NumberMatcher : ILexerMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return c == '-' && char.IsDigit(lexer.Peek(1));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;
        var isFloatingPoint = false;
        
        AdvanceNumber(lexer, ref index, ref column);

        if (lexer.Peek(0) == '.')
        {
            index++;
            column++;
            
            AdvanceNumber(lexer, ref index, ref column);
            isFloatingPoint = true;
        }

        return new(isFloatingPoint ? PredefinedSymbols.FloatingPoint : PredefinedSymbols.SignedInteger, lexer.Document.Source.Substring(oldIndex, index - oldIndex),
            line, oldColumn);
    }

    private static void AdvanceNumber(Lexer lexer, ref int index, ref int column)
    {
        do
        {
            index++;
            column++;
        } while (index < lexer.Document.Source.Length && char.IsDigit(lexer.Document.Source[index]));
    }
}
