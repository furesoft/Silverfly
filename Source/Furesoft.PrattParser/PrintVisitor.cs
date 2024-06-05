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
            LiteralNode<bool> literal => Visit(literal),
            LiteralNode<ulong> literal => Visit(literal),
            LiteralNode<long> literal => Visit(literal),
            LiteralNode<double> literal => Visit(literal),
            LiteralNode<string> literal => Visit(literal),

            _ => ""
        };
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

        builder.Append(call.FunctionExpr.Accept(this));
        builder.Append('(');

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

    public string Visit(LiteralNode<bool> literalNode)
    {
        return literalNode.Value.ToString();
    }

    public string Visit(LiteralNode<double> literalNode)
    {
        return literalNode.Value.ToString(CultureInfo.InvariantCulture);
    }

    public string Visit(LiteralNode<string> literalNode)
    {
        return $"'{literalNode.Value}'";
    }

    public string Visit(LiteralNode<ulong> literalNode)
    {
        return literalNode.Value.ToString();
    }

    public string Visit(LiteralNode<long> literalNode)
    {
        return literalNode.Value.ToString();
    }
}
