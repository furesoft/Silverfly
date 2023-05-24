namespace Furesoft.PrattParser.Matcher;

public class NumberMatcher : ILexerMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return c == '-' && char.IsDigit(lexer.Peek(1)) || char.IsDigit(lexer.Peek(0));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;
        
        AdvanceNumber(lexer, ref index, ref column);

        // Handle Floating Point Numbers
        if (lexer.Peek(0) == '.')
        {
            lexer.Advance();
            
            AdvanceNumber(lexer, ref index, ref column);

            // Handle E-Notation
            if (lexer.Peek(0) == 'e' || lexer.Peek(0) == 'E')
            {
                lexer.Advance();
                
                AdvanceNumber(lexer, ref index, ref column);
            }
        }

        return new(PredefinedSymbols.Number, lexer.Document.Source.Substring(oldIndex, index - oldIndex),
            line, oldColumn);
    }

    private static void AdvanceNumber(Lexer lexer, ref int index, ref int column)
    {
        do
        {
            lexer.Advance();;
        } while (index < lexer.Document.Source.Length && char.IsDigit(lexer.Document.Source[index]));
    }
}
