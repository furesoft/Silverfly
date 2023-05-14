namespace Furesoft.PrattParser;

public interface ILexer<TokenType>
{
    Token<TokenType> Next();
}
