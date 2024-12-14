using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class TupleNode : AstNode
{
    public TupleNode(ImmutableList<AstNode> values)
    {
        Children.Add(values);
    }
}
