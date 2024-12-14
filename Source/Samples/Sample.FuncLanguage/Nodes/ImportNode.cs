using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class ImportNode : AstNode
{
    public ImportNode(string path)
    {
        Properties.Set(nameof(Path), path);
    }

    public string Path
    {
        get => Properties.GetOrThrow<string>(nameof(Path));
    }
}
