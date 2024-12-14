using System.Collections.Immutable;
using System.Linq;
using System.Text;
using MrKWatkins.Ast.Listening;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly;

/// <summary>
///     A visitor class that processes nodes in an abstract syntax tree (AST) and produces a string representation of each
///     node.
/// </summary>
/// <remarks>
///     This class derives from <see cref="NodeVisitor{T}" /> with <typeparamref name="string" /> as the return type.
///     It provides implementations for visiting different types of nodes in the AST and generates a string output for each
///     node.
///     The actual behavior and string formatting are defined in the methods of this class that override the base class
///     methods.
/// </remarks>
public static class PrintListener
{
    public static CompositeListener<StringBuilder, AstNode> Listener = CompositeListener<StringBuilder, AstNode>
        .Build()
        .With(new GroupListener())
        .With(new InvalidListener())
        .With(new NameListener())
        .With(new LiteralListener())
        .With(new BinaryListener())
        .With(new PrefixListener())
        .With(new PostfixListener())
        .With(new TernaryListener())
        .With(new CallListener())
        .With(new BlockListener())
        .With(new UnknownListener())
        .ToListener();

    private class GroupListener : Listener<StringBuilder, AstNode, GroupNode>
    {
        protected override void ListenToNode(StringBuilder context, GroupNode group)
        {
            context.Append(group.LeftSymbol);
            Listen(context, group.Expr);
            context.Append(group.RightSymbol);
        }
    }

    private class InvalidListener : Listener<StringBuilder, AstNode, InvalidNode>
    {
        protected override void ListenToNode(StringBuilder context, InvalidNode node)
        {
            context.Append($"#Invalid({node.Token})");
        }
    }

    private class NameListener : Listener<StringBuilder, AstNode, NameNode>
    {
        protected override void ListenToNode(StringBuilder context, NameNode node)
        {
            context.Append(node.Token.Text.ToString());
        }
    }

    private class LiteralListener : Listener<StringBuilder, AstNode, LiteralNode>
    {
        protected override void ListenToNode(StringBuilder builder, LiteralNode node)
        {
            if (node.Value is ImmutableList<AstNode> list)
            {
                builder.Append('(');

                for (var i = 0; i < list.Count; i++)
                {
                    Listen(builder, list[i]);
                    if (i < list.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }

                builder.Append(')');
            }

            builder.Append(node.Value);
        }
    }

    private class BinaryListener : Listener<StringBuilder, AstNode, BinaryOperatorNode>
    {
        protected override void ListenToNode(StringBuilder builder, BinaryOperatorNode node)
        {
            builder.Append('(');

            Listen(builder, node.Left);
            builder.Append(' ').Append(node.Operator.Type.Punctuator()).Append(' ');

            Listen(builder, node.Right);
            builder.Append(')');
        }
    }

    private class PrefixListener : Listener<StringBuilder, AstNode, PrefixOperatorNode>
    {
        protected override void ListenToNode(StringBuilder builder, PrefixOperatorNode node)
        {
            builder.Append('(');
            builder.Append(node.Operator.Type.Punctuator());

            if (char.IsLetter(node.Operator.Type.Punctuator()[0]))
            {
                builder.Append(' ');
            }

            Listen(builder, node.Expr);
            builder.Append(')');
        }
    }

    private class PostfixListener : Listener<StringBuilder, AstNode, PostfixOperatorNode>
    {
        protected override void ListenToNode(StringBuilder builder, PostfixOperatorNode node)
        {
            builder.Append('(');
            Listen(builder, node.Expr);

            if (char.IsLetter(node.Operator.Type.Punctuator()[0]))
            {
                builder.Append(' ');
            }

            builder.Append(node.Operator.Type.Punctuator()).Append(')');
        }
    }

    private class TernaryListener : Listener<StringBuilder, AstNode, TernaryOperatorNode>
    {
        protected override void ListenToNode(StringBuilder builder, TernaryOperatorNode node)
        {
            builder.Append('(');
            Listen(builder, node.FirstExpr);
            builder.Append(" ? ");

            Listen(builder, node.SecondExpr);
            builder.Append(" : ");
            Listen(builder, node.ThirdExpr);

            builder.Append(')');
        }
    }

    private class CallListener : Listener<StringBuilder, AstNode, CallNode>
    {
        protected override void ListenToNode(StringBuilder builder, CallNode node)
        {
            builder.Append('(');

            builder.Append('(');
            Listen(builder, node.FunctionExpr);
            builder.Append(')');

            if (node.Arguments.Any())
            {
                builder.Append(' ');
            }

            for (var i = 0; i < node.Arguments.Count(); i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                Listen(builder, node.Arguments.Skip(i).First());
            }

            builder.Append(')');
        }
    }

    private class BlockListener : Listener<StringBuilder, AstNode, BlockNode>
    {
        protected override void ListenToNode(StringBuilder builder, BlockNode node)
        {
            for (var i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                Listen(builder, child);

                if (i % 2 == 0)
                {
                    builder.Append(node.SeperatorSymbol.Name + " ");
                }
            }
        }
    }

    private class UnknownListener : Listener<StringBuilder, AstNode>
    {
        protected override void ListenToNode(StringBuilder builder, AstNode node)
        {
            builder.Append('(');
            builder.Append(node.GetType().Name);

            var properties = node.GetType().GetProperties();
            var firstProperty = true;

            foreach (var property in properties)
            {
                var value = property.GetValue(node);

                if (value == null)
                {
                    continue; // Skip null values
                }

                if (property.PropertyType == typeof(Token))
                {
                    if (!firstProperty)
                    {
                        builder.Append(',');
                    }

                    builder.Append($" {property.Name}={(Token)value}");
                    firstProperty = false;
                    continue;
                }

                if (property.PropertyType == typeof(ImmutableList<AstNode>))
                {
                    var list = value as ImmutableList<AstNode>;
                    if (list != null)
                    {
                        if (!firstProperty)
                        {
                            builder.Append(',');
                        }

                        builder.Append($" {property.Name}=[");

                        for (var i = 0; i < list.Count; i++)
                        {
                            Listen(builder, list[i]);
                            if (i < list.Count - 1)
                            {
                                builder.Append(", ");
                            }
                        }

                        builder.Append(']');
                        firstProperty = false;
                    }
                    else
                    {
                        if (!firstProperty)
                        {
                            builder.Append(',');
                        }

                        builder.Append($" {property.Name}=null");
                        firstProperty = false;
                    }
                }

                if (property.PropertyType == typeof(AstNode))
                {
                    if (!firstProperty)
                    {
                        builder.Append(',');
                    }

                    builder.Append($" {property.Name}=");
                    Listen(builder, (AstNode)value);
                    firstProperty = false;
                }
            }

            builder.Append(')');
        }
    }
}
