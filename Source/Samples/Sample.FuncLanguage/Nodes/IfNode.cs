using Silverfly.Nodes;

namespace Sample.FuncLanguage.Nodes;

public record IfNode(AstNode Condition, AstNode TruePart, AstNode FalsePart) : AstNode;