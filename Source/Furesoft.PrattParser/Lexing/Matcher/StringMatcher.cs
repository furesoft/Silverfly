namespace Furesoft.PrattParser.Lexing.Matcher;

public class StringMatcher(Symbol leftStr, Symbol rightStr) : ILexerMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return leftStr != null && rightStr != null &&
                             lexer.IsMatch(leftStr.Name);
    }

    public Token Build(Lexer lexer, ref int index, ref int column, ref int line)
    {
        var oldColumn = column;
        var oldIndex = index;

        index++;
        column++;

        while (!lexer.IsMatch(rightStr.Name))
        {
            lexer.Advance();
        }

        var text = lexer.Document.Source.Slice(oldIndex + 1, index - oldIndex - 1);

        index++;
        column++;


        return new(PredefinedSymbols.String, text, line, oldColumn);
    }
}
