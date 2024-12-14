using Silverfly.Nodes;

namespace Sample.JSON.Nodes;

public class JsonObject : AstNode
{
    public JsonObject(Dictionary<string, AstNode> members)
    {
        Properties.Set(nameof(Members), members);
    }

    public Dictionary<string, AstNode> Members
    {
        get => Properties.GetOrThrow<Dictionary<string, AstNode>>(nameof(Members));
    }
}
