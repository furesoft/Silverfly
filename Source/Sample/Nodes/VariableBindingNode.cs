using Furesoft.PrattParser.Nodes;

namespace Sample.Nodes;


public record VariableBindingNode(Token Name, Expression Value) : StatementNode
{

}