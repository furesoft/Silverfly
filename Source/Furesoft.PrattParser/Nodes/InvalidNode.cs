namespace Furesoft.PrattParser.Nodes;

public class InvalidNode : AstNode
{
    public Token Token { get; }

    public InvalidNode(Token token)
    {
        Token = token;
    }
}
