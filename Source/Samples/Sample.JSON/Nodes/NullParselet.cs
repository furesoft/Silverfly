﻿using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.JSON.Nodes;

public class NullParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        return new LiteralNode(null);
    }
}
