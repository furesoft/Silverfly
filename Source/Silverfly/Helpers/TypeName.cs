using Silverfly.Nodes;

namespace Silverfly.Helpers;

public class TypeName : AstNode
{
    public TypeName(Token token)
    {
        Properties.Set(nameof(Token), token);
    }

    public Token Token => Properties.GetOrThrow<Token>(nameof(Token));

    public override string ToString() => Token.ToString();
}
