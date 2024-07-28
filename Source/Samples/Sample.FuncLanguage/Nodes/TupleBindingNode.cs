using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record TupleBindingNode(ImmutableList<NameNode> Names, AstNode Value) : AnnotatedNode;
