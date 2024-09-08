using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Rockstar.Nodes;

public record LoopNode(AstNode Condition, ImmutableList<AstNode> Body) : StatementNode;
