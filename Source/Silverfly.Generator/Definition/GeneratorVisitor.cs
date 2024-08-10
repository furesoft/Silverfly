using System.Text;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Generator.Definition;

public class GeneratorVisitor : NodeVisitor
{
    private readonly StringBuilder _builder;
    private int _indentationLevel = 1;
    private const string IndentationString = "    "; // 4 spaces for each indentation level

    public GeneratorVisitor(StringBuilder builder)
    {
        _builder = builder;

        For<NameNode>(Visit);
        For<LiteralNode>(Visit);
        For<GroupNode>(Visit);
        For<BlockNode>(Visit);
    }

    private void AppendWithIndentation(string text)
    {
        _builder.Append(string.Concat(Enumerable.Repeat(IndentationString, _indentationLevel)));
        _builder.Append(text);
    }

    private void Visit(LiteralNode name)
    {
        AppendWithIndentation($"parser.Consume(\"{name.Value}\");\n");
    }

    private void Visit(NameNode name)
    {
        switch (name.Token.Text.ToString())
        {
            case "id":
                AppendWithIndentation("parser.Consume(PredefinedSymbols.Name);\n");
                break;
            case "expr":
                AppendWithIndentation("parser.ParseExpression();\n");
                break;
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
                AppendWithIndentation("parser.ParseExpr();\n");
            }
        }

        if (group.Expr is BinaryOperatorNode bin && bin.Operator.Text.Span == ":")
        {
            AppendWithIndentation($"var {GetName(bin.RightExpr)} = ");
            bin.LeftExpr.Accept(this);
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
        BeginBlock();
        foreach (var child in block.Children)
        {
            child.Accept(this);
        }
        EndBlock();
    }

    private void EndBlock()
    {
        _indentationLevel--;
        AppendWithIndentation("}\n");
    }

    private void BeginBlock()
    {
        AppendWithIndentation("{\n");
        _indentationLevel++;
    }

    public override string ToString() => _builder.ToString();
}
