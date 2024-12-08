using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly;

/// <summary>
/// A visitor class that processes nodes in an abstract syntax tree (AST) and produces a string representation of each node.
/// </summary>
/// <remarks>
/// This class derives from <see cref="NodeVisitor{T}"/> with <typeparamref name="string"/> as the return type.
/// It provides implementations for visiting different types of nodes in the AST and generates a string output for each node.
/// The actual behavior and string formatting are defined in the methods of this class that override the base class methods.
/// </remarks>
public class PrintVisitor : NodeVisitor<string>
{
    public PrintVisitor()
    {
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

    private string Visit(GroupNode group)
    {
        return group.LeftSymbol + Visit(group.Expr) + group.RightSymbol;
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
        var firstProperty = true;

        foreach (var property in properties)
        {
            var value = property.GetValue(node);

            if (value == null) continue; // Skip null values

            if (property.PropertyType == typeof(Token))
            {
                if (!firstProperty) builder.Append(',');
                builder.Append($" {property.Name}={(Token)value}");
                firstProperty = false;
                continue;
            }

            if (property.PropertyType == typeof(ImmutableList<AstNode>))
            {
                var list = (ImmutableList<AstNode>)value;
                if (!firstProperty) builder.Append(',');
                builder.Append($" {property.Name}=[{string.Join(", ", list!.Select(Visit))}]");
                firstProperty = false;
                continue;
            }

            if (property.PropertyType == typeof(AstNode))
            {
                if (!firstProperty) builder.Append(',');
                builder.Append($" {property.Name}={Visit((AstNode)value)}");
                firstProperty = false;
            }
        }

        builder.Append(')');

        return builder.ToString();
    }

    public string Visit(BlockNode block)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < block.Children.Count; i++)
        {
            var child = block.Children[i];
            builder.Append(child.Accept(this));

            if (i % 2 == 0)
            {
                builder.Append(block.SeperatorSymbol.Name + " ");
            }
        }

        return builder.ToString();
    }

    public string Visit(CallNode call)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append('(' + call.FunctionExpr.Accept(this) + ")");

        if (call.Arguments.Any())
        {
            builder.Append(' ');
        }

        for (var i = 0; i < call.Arguments.Count(); i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(call.Arguments.Skip(i).First().Accept(this));
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
        builder.Append(binary.Left.Accept(this));
        builder.Append(' ').Append(binary.Operator.Type.Punctuator()).Append(' ');
        builder.Append(binary.Right.Accept(this));
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
