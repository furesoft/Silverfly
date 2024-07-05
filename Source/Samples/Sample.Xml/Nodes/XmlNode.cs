using Silverfly.Nodes;
using System.Collections.Immutable;

namespace Sample.Xml.Nodes;

public record XmlNode(string Tag, ImmutableList<AstNode> Attributes, ImmutableList<AstNode> Children, string InnerText) : AstNode;