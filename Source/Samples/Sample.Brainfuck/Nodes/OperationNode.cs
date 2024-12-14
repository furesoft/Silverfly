using Silverfly;
using Silverfly.Nodes;

namespace Sample.Brainfuck.Nodes;

public abstract class OperationNode : AstNode
{
    public OperationNode(Token token)
    {
        Properties.Set(nameof(Token), token);
    }

    public Token Token
    {
        get => Properties.GetOrThrow<Token>(nameof(Token));
        set => Properties.Set(nameof(Token), value);
    }
}
