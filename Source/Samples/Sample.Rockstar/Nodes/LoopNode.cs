using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Rockstar.Nodes;

public class LoopNode : StatementNode
{
    public AstNode Condition => Children[0];
    public IEnumerable<AstNode> Body => Children.Skip(1);

    public LoopNode(AstNode condition, ImmutableList<AstNode> body)
    {
        Children.Add(condition);
        Children.Add(body);
    }
}
