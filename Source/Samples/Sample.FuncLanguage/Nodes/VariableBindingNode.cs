using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class VariableBindingNode : AnnotatedNode
{
    public VariableBindingNode(Token name, IEnumerable<NameNode> parameters, AstNode value)
    {
        Properties.Set(nameof(Name), name);

        Children.Add(value);
        Children.Add(parameters);
    }

    public Token Name
    {
        get => Properties.GetOrThrow<Token>(nameof(Name));
    }

    public AstNode Value
    {
        get => Children.First();
    }

    public IEnumerable<AstNode> Parameters
    {
        get => Children.Skip(1);
    }
}
