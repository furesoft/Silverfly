using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record TupleNode(ImmutableList<AstNode> Values) : AstNode;