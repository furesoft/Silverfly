using System;
using System.Globalization;
using System.Text;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Furesoft.PrattParser;

public class PrintVisitor : IVisitor<string>
{
    public virtual string Visit(AstNode node)
    {
        return node switch
        {
            CallNode call => Visit(call),
            TernaryOperatorNode cond => Visit(cond),
            BlockNode block => Visit(block),
            NameNode name => Visit(name),
            BinaryOperatorNode op => Visit(op),
            PostfixOperatorNode postfix => Visit(postfix),
            PrefixOperatorNode prefix => Visit(prefix),
            LiteralNode literal => Visit(literal),

            _ => VisitOther(node)
        };
    }

    private string VisitOther(AstNode node)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(node.GetType().Name);

        var properties = node.GetType().GetProperties();
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(Token))
            {
                builder.Append($" {property.Name}={(Token)property.GetValue(node)}");
                continue;
            }

            if (property.PropertyType != typeof(AstNode) || property.GetValue(node) == null) continue;

            builder.Append(' ');
            builder.Append(property.Name);
            builder.Append('=');
            builder.Append(Visit((AstNode)property.GetValue(node)));
            builder.Append(',');
        }

        // remove the trailing comma
        builder.Length -= 1;

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

        if (call.Arguments.Count > 0)
        {
            builder.Append(' ');
        }

        for (var i = 0; i < call.Arguments.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(call.Arguments[i].Accept(this));
        }

        builder.Append(')');

        return builder.ToString();
    }

    public string Visit(TernaryOperatorNode cond)
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
        return name.Name;
    }

    public string Visit(BinaryOperatorNode binary)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(binary.LeftExpr.Accept(this));
        builder.Append(' ').Append(binary.Operator.Punctuator()).Append(' ');
        builder.Append(binary.RightExpr.Accept(this));
        builder.Append(')');

        return builder.ToString();
    }

    public string Visit(PostfixOperatorNode postfix)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(postfix.Expr.Accept(this));

        if (char.IsLetter(postfix.Operator.Punctuator()[0]))
        {
            builder.Append(' ');
        }

        builder.Append(postfix.Operator.Punctuator()).Append(')');

        return builder.ToString();
    }

    public string Visit(PrefixOperatorNode prefix)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(prefix.Operator.Punctuator());

        if (char.IsLetter(prefix.Operator.Punctuator()[0]))
        {
            builder.Append(' ');
        }

        builder.Append(prefix.Expr.Accept(this)).Append(')');

        return builder.ToString();
    }

    public string Visit(LiteralNode literalNode)
    {
        return literalNode.Value.ToString();
    }
}
