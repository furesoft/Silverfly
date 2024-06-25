using System;
using System.Collections.Immutable;

namespace Furesoft.PrattParser.Nodes;

public record BlockNode(Symbol SeperatorSymbol, Symbol Terminator) : AstNode
{
    public ImmutableList<AstNode> Children { get; set; }

    public BlockNode WithChildren(ImmutableList<AstNode> nodes)
    {
        Children = nodes;

        return this;
    }
}
