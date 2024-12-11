using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class LambdaNode : AstNode
{
    public AstNode Value => Children.First;
    public IEnumerable<NameNode> Parameters => Children.Skip(1).OfType<NameNode>();

    public LambdaNode(ImmutableList<NameNode> parameters, AstNode value)
    {
        Children.Add(value);
        Children.Add(parameters);
    }
}
