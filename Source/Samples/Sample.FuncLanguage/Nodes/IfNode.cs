using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public record IfNode(AstNode Condition, AstNode TruePart, AstNode FalsePart) : AstNode;