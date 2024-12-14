using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class LambdaNode : AstNode
{
    public LambdaNode(ImmutableList<NameNode> parameters, AstNode value)
    {
        Children.Add(value);
        Children.Add(parameters);
    }

    public AstNode Value
    {
        get => Children.First;
    }

    public IEnumerable<NameNode> Parameters
    {
        get => Children.Skip(1).OfType<NameNode>();
    }
}
