using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class IfNode : AstNode
{
    public IfNode(AstNode condition, AstNode truePart, AstNode falsePart)
    {
        Children.Add(condition);
        Children.Add(truePart);
        Children.Add(falsePart);
    }

    public AstNode Condition
    {
        get => Children.First;
    }

    public AstNode TruePart
    {
        get => Children[0];
    }

    public AstNode FalsePart
    {
        get => Children.Last;
    }
}
