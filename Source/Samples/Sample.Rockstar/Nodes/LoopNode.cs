using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Rockstar.Nodes;

public class LoopNode : StatementNode
{
    public LoopNode(AstNode condition, ImmutableList<AstNode> body)
    {
        Children.Add(condition);
        Children.Add(body);
    }

    public AstNode Condition
    {
        get => Children[0];
    }

    public IEnumerable<AstNode> Body
    {
        get => Children.Skip(1);
    }
}
