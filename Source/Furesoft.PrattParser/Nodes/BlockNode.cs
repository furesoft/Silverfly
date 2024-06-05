using System.Collections.Generic;

namespace Furesoft.PrattParser.Nodes;

public class BlockNode(Symbol seperator, List<AstNode> children) : AstNode
{
    public Symbol SeperatorSymbol { get; set; } = seperator;
    public List<AstNode> Children { get; set; } = children;
}
