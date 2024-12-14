using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class TupleBindingNode : AnnotatedNode
{
    public TupleBindingNode(ImmutableList<NameNode> names, AstNode value)
    {
        Children.Add(value);
        Children.Add(names);
    }

    public AstNode Value
    {
        get => Children.First;
    }

    public NameNode[] Names
    {
        get => Children.Skip(1).OfType<NameNode>().ToArray();
    }
}
