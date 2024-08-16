using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Rockstar.Parselets;

public class MappingParselet(object Value) : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(Value, token).WithRange(token);
    }
}
