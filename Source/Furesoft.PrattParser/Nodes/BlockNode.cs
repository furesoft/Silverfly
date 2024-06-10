using System;
using System.Collections.Generic;

namespace Furesoft.PrattParser.Nodes;

public class BlockNode(Symbol seperator, Symbol terminator) : AstNode
{
    public Symbol SeperatorSymbol { get; set; } = seperator;
    public Symbol Terminator { get; } = terminator;
    public List<AstNode> Children { get; set; } = [];

    public BlockNode WithChildren(List<AstNode> nodes)
    {
        Children = nodes;

        return this;
    }
}
