using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;

namespace Sample.Nodes;

public record VariableBindingNode(Token Name, AstNode Value) : StatementNode
{

}