using Furesoft.PrattParser.Nodes;

namespace TestProject;

public class IfNode : AstNode
{
    public AstNode Cond { get; set; }
    public BlockNode Body { get; set; }
    public BlockNode ElseBody { get; set; }
}
