using System;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder;

public class BuilderParselet<TNode>(int bindingPower, SyntaxElement definition) : IInfixParselet
    where TNode : AstNode
{
    public int GetBindingPower() => bindingPower;

    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        definition.Parse(parser);

        return Activator.CreateInstance<TNode>();
    }
}