using Silverfly.Nodes;

namespace Sample.Nodes;

public record IfNode(AstNode Condition, AstNode TruePart, AstNode FalsePart) : AstNode;