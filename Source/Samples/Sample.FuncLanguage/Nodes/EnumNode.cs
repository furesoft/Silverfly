using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Sivlerfly.Sample.FuncLanguage.Nodes;

public class EnumNode : AstNode
{
    public EnumNode(string name, ImmutableList<AstNode> members)
    {
        Properties.Set(nameof(Name), name);

        Children.Add(members);
    }

    public string Name
    {
        get => Properties.GetOrThrow<string>(nameof(Name));
    }
}
