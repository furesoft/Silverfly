using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class IfNode : AstNode
{
    public AstNode Condition => Children.First;
    public AstNode TruePart => Children[0];
    public AstNode FalsePart => Children.Last;

    public IfNode(AstNode condition, AstNode truePart, AstNode falsePart)
    {
        Children.Add(condition);
        Children.Add(truePart);
        Children.Add(falsePart);
    }
}
