using Silverfly.Nodes;

namespace Sample.FuncLanguage.Nodes;

public record ImportNode(string Path) : AstNode;