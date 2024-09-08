using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Rockstar.Nodes;

public record IfNode(AstNode Condition, ImmutableList<AstNode> TruePart, ImmutableList<AstNode> FalsePart)
    : StatementNode;
