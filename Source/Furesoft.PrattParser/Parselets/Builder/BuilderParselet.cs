using System;
using System.Collections;
using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder;

public class BuilderParselet<TNode>(int bindingPower, SyntaxElement definition) : IPrefixParselet
    where TNode : AstNode
{
    public int GetBindingPower() => bindingPower;

    public AstNode Parse(Parser parser, Token token)
    {
        var result = new List<AstNode>();

        definition.CurrentToken = token;
        definition.Parse(parser, result);

        var node = (TNode)Activator.CreateInstance(typeof(TNode), [.. result]);

        return node.WithRange(token, parser.LookAhead(0));
    }
}