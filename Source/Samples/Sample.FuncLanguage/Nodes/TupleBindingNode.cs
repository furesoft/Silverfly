using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class TupleBindingNode : AnnotatedNode
{
    public AstNode Value => Children.First;
    public NameNode[] Names => Children.Skip(1).OfType<NameNode>().ToArray();

    public TupleBindingNode(ImmutableList<NameNode> names, AstNode value)
    {
        Children.Add(value);
        Children.Add(names);
    }
}

