using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Text.Formatting;

public class ConfigurablePrintVisitor : NodeVisitor<string>
{
    private readonly FormatterConfig _config;
    private int _indentLevel;

    public ConfigurablePrintVisitor(FormatterConfig config)
    {
        _config = config;

        For<GroupNode>(Visit);
        For<BlockNode>(Visit);
        For<TernaryOperatorNode>(VisitTernary);
        For<CallNode>(Visit);
        For<NameNode>(Visit);
        For<BinaryOperatorNode>(Visit);
        For<PrefixOperatorNode>(Visit);
        For<PostfixOperatorNode>(Visit);
        For<LiteralNode>(Visit);
        For<InvalidNode>(Visit);
    }

    private string Indent()
    {
        var indentCharacter = _config.UseTabs ? "\t" : new string(' ', _config.IndentSize);
        return string.Concat(Enumerable.Repeat(indentCharacter, _indentLevel));
    }

    private string Visit(GroupNode group)
    {
        var spaceWithinParens = _config.SpaceWithinParens ? " " : "";
        return group.LeftSymbol + spaceWithinParens + Visit(group.Expr) + spaceWithinParens + group.RightSymbol;
    }

    protected string Visit(InvalidNode invalid)
    {
        return $"#Invalid({invalid.Token})";
    }

    protected override string VisitUnknown(AstNode node)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(node.GetType().Name);

        var properties = node.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(node);

            if (property.PropertyType == typeof(Token))
            {
                builder.Append($" {property.Name}={(Token)value!}");
                continue;
            }

            if (property.PropertyType == typeof(ImmutableList<AstNode>))
            {
                var list = (ImmutableList<AstNode>)value;
                builder.Append($" {property.Name}={string.Join(',', list!.Select(Visit))}");
                continue;
            }

            if (property.PropertyType != typeof(AstNode) || property.GetValue(node) == null)
            {
                continue;
            }

            builder.Append(' ');
            builder.Append(property.Name);
            builder.Append('=');

            builder.Append(Visit((AstNode)value));

            builder.Append(',');
        }

        builder.Length -= 1; // Remove the trailing comma
        builder.Append(')');

        return builder.ToString();
    }

    public string Visit(BlockNode block)
    {
        var builder = new StringBuilder();

        if (_config.BraceStyle == BraceStyle.NextLine)
        {
            builder.AppendLine();
            builder.Append(Indent()).AppendLine("{");
        }
        else
        {
            builder.Append(" {");
        }

        _indentLevel++;

        for (var i = 0; i < block.Children.Count; i++)
        {
            var child = block.Children[i];
            builder.Append(Indent());
            builder.Append(child.Accept(this));
            builder.AppendLine(); // Each statement in a new line
        }

        _indentLevel--;

        builder.Append(Indent()).Append("}");
        return builder.ToString();
    }

    public string Visit(CallNode call)
    {
        var builder = new StringBuilder();

        builder.Append(call.FunctionExpr.Accept(this));

        if (_config.SpaceBeforeParens)
        {
            builder.Append(' ');
        }

        builder.Append('(');
        var spaceWithinParens = _config.SpaceWithinParens ? " " : "";

        for (var i = 0; i < call.Arguments.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(spaceWithinParens);
            builder.Append(call.Arguments[i].Accept(this));
            builder.Append(spaceWithinParens);
        }

        builder.Append(')');
        return builder.ToString();
    }

    public string VisitTernary(TernaryOperatorNode cond)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(cond.FirstExpr.Accept(this));
        builder.Append(" ? ");
        builder.Append(cond.SecondExpr.Accept(this));
        builder.Append(" : ");
        builder.Append(cond.ThirdExpr.Accept(this));
        builder.Append(')');

        return builder.ToString();
    }

    public string Visit(NameNode name)
    {
        return name.Token.Text.ToString();
    }

    public string Visit(BinaryOperatorNode binary)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(binary.LeftExpr.Accept(this));
        builder.Append(' ').Append(binary.Operator.Type.Punctuator()).Append(' ');
        builder.Append(binary.RightExpr.Accept(this));
        builder.Append(')');

        return builder.ToString();
    }

    public string Visit(PostfixOperatorNode postfix)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(postfix.Expr.Accept(this));

        if (char.IsLetter(postfix.Operator.Type.Punctuator()[0]))
        {
            builder.Append(' ');
        }

        builder.Append(postfix.Operator.Type.Punctuator()).Append(')');

        return builder.ToString();
    }

    public string Visit(PrefixOperatorNode prefix)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(prefix.Operator.Type.Punctuator());

        if (char.IsLetter(prefix.Operator.Type.Punctuator()[0]))
        {
            builder.Append(' ');
        }

        builder.Append(prefix.Expr.Accept(this)).Append(')');

        return builder.ToString();
    }

    public string Visit(LiteralNode literalNode)
    {
        if (literalNode.Value is ImmutableList<AstNode> list)
        {
            return $"({string.Join(',', list.Select(Visit))})";
        }

        return literalNode.Value.ToString();
    }
}
