using Silverfly.Nodes;
using System.Collections.Immutable;

namespace Sample.JSON.Nodes;

public class JsonArray : AstNode
{
    public JsonArray(ImmutableList<AstNode> values)
    {
        Children.Add(values);
    }
}

