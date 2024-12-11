using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Rockstar.Nodes;

public class IfNode : StatementNode
{
    public ImmutableList<AstNode> TruePart => Properties.GetOrThrow<ImmutableList<AstNode>>(nameof(TruePart));
    public ImmutableList<AstNode> FalsePart => Properties.GetOrThrow<ImmutableList<AstNode>>(nameof(FalsePart));

    public IfNode(AstNode condition, ImmutableList<AstNode> truePart, ImmutableList<AstNode> falsePart)
    {
        Children.Add(condition);

        Properties.Set(nameof(TruePart), truePart);
        Properties.Set(nameof(FalsePart), falsePart);
    }
}
