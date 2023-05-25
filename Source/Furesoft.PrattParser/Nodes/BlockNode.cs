using System.Collections.Generic;

namespace Furesoft.PrattParser.Nodes;

public class BlockNode : AstNode
{
    public Symbol SeperatorSymbol { get; set; }
    public List<AstNode> Children { get; set; } = new();
}
