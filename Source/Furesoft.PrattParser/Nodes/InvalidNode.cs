namespace Furesoft.PrattParser.Nodes;

public class InvalidNode(Token token) : AstNode
{
    public Token Token { get; } = token;
}
