using System.Collections.Immutable;
using Silverfly.Nodes;

namespace Sivlerfly.Sample.FuncLanguage.Nodes;

public record EnumNode(string Name, ImmutableList<AstNode> Members) : AstNode
{
}