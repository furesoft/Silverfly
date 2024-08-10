using System.Text;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Generator.Definition;

public class GeneratorVisitor : NodeVisitor
{
    private readonly StringBuilder _builder;

    public GeneratorVisitor(StringBuilder builder)
    {
        _builder = builder;

        For<NameNode>(Visit);
        For<LiteralNode>(Visit);
        For<GroupNode>(Visit);
        For<BlockNode>(Visit);
    }

    private void Visit(LiteralNode name)
    {
        _builder.Append($"parser.Consume(\"{name.Value}\");");
    }

    private void Visit(NameNode name)
    {
        if (name.Token.Text.ToString() == "id")
        {
            _builder.Append("parser.Consume(PredefinedSymbols.Name)");
        }
    }

    private void Visit(GroupNode group)
    {
        if (group.LeftSymbol != "<" || group.RightSymbol != ">")
        {
            return;
        }

        if (group.Expr is NameNode nonterminal)
        {
            if (nonterminal.Token.Text.Span == "expr")
            {
                _builder.Append("parser.ParseExpr()");
            }
        }

        if (group.Expr is BinaryOperatorNode bin && bin.Operator.Text.Span == ":")
        {
            _builder.Append($"\nvar {GetName(bin.RightExpr)} = ");
            bin.LeftExpr.Accept(this);
            _builder.AppendLine(";");
        }
    }

    string GetName(AstNode node)
    {
        if (node is NameNode name)
        {
            return name.Token.Text.ToString();
        }

        return null;
    }

    private new void Visit(BlockNode block)
    {
        foreach (var child in block.Children)
        {
            _builder.Append('\t');
            child.Accept(this);
        }
    }

    public override string ToString() => _builder.ToString();
}
