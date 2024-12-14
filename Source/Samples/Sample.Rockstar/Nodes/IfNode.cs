using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Rockstar.Nodes;

public class IfNode : StatementNode
{
    public IfNode(AstNode condition, ImmutableList<AstNode> truePart, ImmutableList<AstNode> falsePart)
    {
        Children.Add(condition);

        Properties.Set(nameof(TruePart), truePart);
        Properties.Set(nameof(FalsePart), falsePart);
    }

    public ImmutableList<AstNode> TruePart
    {
        get => Properties.GetOrThrow<ImmutableList<AstNode>>(nameof(TruePart));
    }

    public ImmutableList<AstNode> FalsePart
    {
        get => Properties.GetOrThrow<ImmutableList<AstNode>>(nameof(FalsePart));
    }
}
