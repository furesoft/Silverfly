using Furesoft.PrattParser.Nodes;

namespace TestProject;

public class IfNode : AstNode
{
    public AstNode Cond { get; set; }
    public AstNode Body { get; set; }
    public AstNode ElseBody { get; set; }
}
