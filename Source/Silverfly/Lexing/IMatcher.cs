namespace Silverfly.Lexing;

public interface IMatcher
{
    bool Match(Lexer lexer, char c);

    Token Build(Lexer lexer, ref int index, ref int column, ref int line);
}
