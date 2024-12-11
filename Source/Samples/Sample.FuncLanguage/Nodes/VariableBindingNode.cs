using Silverfly.Nodes;

namespace Silverfly.Sample.Func.Nodes;

public class VariableBindingNode : AnnotatedNode
{
    public Token Name => Properties.GetOrThrow<Token>(nameof(Name));
    public AstNode Value => Children.First();

    public IEnumerable<AstNode> Parameters => Children.Skip(1);

    public VariableBindingNode(Token name, IEnumerable<NameNode> parameters, AstNode value)
    {
        Properties.Set(nameof(Name), name);

        Children.Add(value);
        Children.Add(parameters);
    }
}
