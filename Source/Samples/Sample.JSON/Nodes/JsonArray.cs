using Silverfly.Nodes;
using System.Collections.Immutable;

namespace Sample.JSON.Nodes;

public record JsonArray(ImmutableList<AstNode> Values) : AstNode;
