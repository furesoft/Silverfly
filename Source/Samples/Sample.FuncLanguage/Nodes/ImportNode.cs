using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class ImportNode : AstNode
{
    public string Path => Properties.GetOrThrow<string>(nameof(Path));

    public ImportNode(string path)
    {
        Properties.Set(nameof(Path), path);
    }
}
