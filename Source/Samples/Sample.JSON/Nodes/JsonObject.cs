using Silverfly.Nodes;

namespace Sample.JSON.Nodes;

public record JsonObject(Dictionary<string, AstNode> Members) : AstNode;
