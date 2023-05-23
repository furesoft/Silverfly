namespace Furesoft.PrattParser;

public interface ILexerPart
{
    bool Match(Lexer lexer, char c);

    Token Build(Lexer lexer, ref int index, ref int column, ref int line);
}
