using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class ModuleNode : AstNode
{
    public ModuleNode(string name)
    {
        Properties.Set(nameof(Name), name);
    }

    public string Name
    {
        get => Properties.GetOrThrow<string>(nameof(Name));
    }
}
