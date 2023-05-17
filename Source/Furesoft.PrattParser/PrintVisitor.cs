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
            AssignNode assign => Visit(assign),
            CallNode call => Visit(call),
            TernaryOperatorNode cond => Visit(cond),
            NameAstNode name => Visit(name),
            BinaryOperatorNode op => Visit(op),
            PostfixOperatorNode postfix => Visit(postfix),
            PrefixOperatorNode prefix => Visit(prefix),
            LiteralNode<bool> literal => Visit(literal),
            LiteralNode<int> literal => Visit(literal),

            _ => ""
        };
    }

    public string Visit(AssignNode assign)
    {
        var name = $"({assign.Name} = ";
        name += assign.ValueExpr.Accept(this);

        return $"{name})";
    }

    public string Visit(CallNode call)
    {
        var sb = new StringBuilder();

        sb.Append(call.FunctionExpr.Accept(this));
        sb.Append('(');

        for (var i = 0; i < call.ArgumentExprs.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }

            sb.Append(call.ArgumentExprs[i].Accept(this));
        }

        sb.Append(')');

        return sb.ToString();
    }

    public string Visit(TernaryOperatorNode cond)
    {
        var sb = new StringBuilder();

        sb.Append('(');
        sb.Append(cond.FirstExpr.Accept(this));
        sb.Append(" ? ");

        sb.Append(cond.SecondExpr.Accept(this));
        sb.Append(" : ");
        sb.Append(cond.ThirdExpr.Accept(this));

        sb.Append(')');

        return sb.ToString();
    }

    public string Visit(NameAstNode name)
    {
        return name.Name;
    }

    public string Visit(BinaryOperatorNode binary)
    {
        var sb = new StringBuilder();

        sb.Append('(');
        sb.Append(binary.LeftExpr.Accept(this));
        sb.Append(' ').Append(binary.Operator.Punctuator()).Append(' ');
        sb.Append(binary.RightExpr.Accept(this));
        sb.Append(')');

        return sb.ToString();
    }

    public string Visit(PostfixOperatorNode postfix)
    {
        var sb = new StringBuilder();

        sb.Append('(');
        sb.Append(postfix.Expr.Accept(this));
        sb.Append(postfix.Operator.Punctuator()).Append(')');

        return sb.ToString();
    }

    public string Visit(PrefixOperatorNode prefix)
    {
        var sb = new StringBuilder();

        sb.Append('(');
        sb.Append(prefix.Operator.Punctuator());
        sb.Append(prefix.Expr.Accept(this)).Append(')');

        return sb.ToString();
    }

    public string Visit(LiteralNode<bool> literalNode)
    {
        return literalNode.Value.ToString();
    }
    
    public string Visit(LiteralNode<int> literalNode)
    {
        return literalNode.Value.ToString();
    }
}
