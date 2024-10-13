using Silverfly.Nodes;

namespace Silverfly.Sample.JSON.Nodes;

public record JsonObject(Dictionary<string, AstNode> Members) : AstNode;
