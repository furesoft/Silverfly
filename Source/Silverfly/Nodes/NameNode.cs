namespace Silverfly.Nodes;

/// <summary>
///     A simple variable name expression like "abc".
/// </summary>
public class NameNode : AstNode
{
    public NameNode(Token token)
    {
        Properties.Set(nameof(Token), token);
    }

    public Token Token
    {
        get => Properties.GetOrThrow<Token>(nameof(Token));
    }
}
