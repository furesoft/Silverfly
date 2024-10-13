using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.JSON.Nodes;

public record JsonArray(ImmutableList<AstNode> Values) : AstNode;
