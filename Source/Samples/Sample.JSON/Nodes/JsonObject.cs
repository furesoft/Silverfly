using Silverfly.Nodes;

namespace Sample.JSON.Nodes;

public record JsonObject(KeyValuePair<string, AstNode> Values) : AstNode;
