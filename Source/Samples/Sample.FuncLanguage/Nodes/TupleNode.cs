using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Sample.FuncLanguage.Nodes;

public record TupleNode(ImmutableList<AstNode> Values) : AstNode;