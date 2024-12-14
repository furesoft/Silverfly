using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Sample.JSON.Nodes;

public class JsonArray : AstNode
{
    public JsonArray(ImmutableList<AstNode> values)
    {
        Children.Add(values);
    }
}
