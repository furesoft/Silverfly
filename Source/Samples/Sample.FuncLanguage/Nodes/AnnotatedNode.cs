using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class AnnotatedNode : AstNode
{
    public AnnotatedNode()
    {
        Properties.Set(nameof(Annotations), new List<CallNode>());
    }

    public List<CallNode> Annotations
    {
        get => Properties.GetOrThrow<List<CallNode>>(nameof(Annotations));
    }

    public bool HasAnnotation(string name)
    {
        return Annotations
            .Where(_ => _.FunctionExpr is NameNode)
            .Select(_ => (NameNode)_.FunctionExpr)
            .Any(_ => _.Token.Text.ToString() == name);
    }
}
