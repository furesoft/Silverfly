using Silverfly.Nodes;

namespace Silverfly.Helpers;

public record TypeName(Token Token) : AstNode
{
    public override string ToString() => Token.ToString();
}
