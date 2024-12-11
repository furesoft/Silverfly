using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class ModuleNode : AstNode
{
    public string Name => Properties.GetOrThrow<string>(nameof(Name));

    public ModuleNode(string name)
    {
        Properties.Set(nameof(Name), name);
    }
}

