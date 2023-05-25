namespace Furesoft.PrattParser.Matcher;

public class NumberMatcher : ILexerMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return lexer.IsMatch("0x") || c == '-' && char.IsDigit(lexer.Peek(1)) || char.IsDigit(lexer.Peek(0));
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        if (lexer.IsMatch("0x"))
        {
            LexHexNumber(lexer, ref index);
            goto createToken;
        }

        AdvanceNumber(lexer, ref index);
        
        LexFloatingPointNumber(lexer, ref index, ref column);
        
        createToken: 
            return new(PredefinedSymbols.Number, lexer.Document.Source.Substring(oldIndex, index - oldIndex),
                line, oldColumn);
    }

    private void LexHexNumber(Lexer lexer, ref int index)
    {
        
            do
            {
                lexer.Advance();
            } while (index < lexer.Document.Source.Length && IsValidHexChar(lexer.Document.Source[index]));
        
    }


    private static void LexFloatingPointNumber(Lexer lexer, ref int index, ref int column)
    {
        if (lexer.Peek(0) == '.')
        {
            lexer.Advance();

            AdvanceNumber(lexer, ref index);

            // Handle E-Notation
            if (lexer.Peek(0) == 'e' || lexer.Peek(0) == 'E')
            {
                lexer.Advance();

                AdvanceNumber(lexer, ref index);
            }
        }
    }

    private static void AdvanceNumber(Lexer lexer, ref int index)
    {
        do
        {
            lexer.Advance();;
        } while (index < lexer.Document.Source.Length && char.IsDigit(lexer.Document.Source[index]));
    }
    
    private bool IsValidHexChar(char c) => char.IsDigit(c) || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
}
