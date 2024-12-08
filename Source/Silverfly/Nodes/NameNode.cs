namespace Silverfly.Nodes;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public class NameNode : AstNode
{
    public Token Token => Properties.GetOrThrow<Token>(nameof(Token));
    public NameNode(Token token)
    {
        Properties.Set(nameof(Token), token);
    }
}
