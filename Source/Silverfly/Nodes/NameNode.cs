namespace Silverfly.Nodes;

/// <summary>
///     A simple variable name expression like "abc".
/// </summary>
public class NameNode : AstNode
{
    public NameNode(Token token)
    {
        Properties.Set(nameof(Token), token);
        Properties.Set(nameof(Identifier), token.Text.ToString());
    }

    public Token Token
    {
        get => Properties.GetOrThrow<Token>(nameof(Token));
    }

    public string Identifier
    {
        get => Properties.GetOrThrow<string>(nameof(Identifier));
    }
}
