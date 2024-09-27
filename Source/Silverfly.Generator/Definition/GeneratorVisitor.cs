using System.Text;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Generator.Definition;

internal class GeneratorVisitor : NodeVisitor
{
    private const string IndentationString = "    "; // 4 spaces for each indentation level
    private readonly StringBuilder _builder;
    public readonly List<string> Names = [];
    private int _indentationLevel = 2;

    public GeneratorVisitor(StringBuilder builder)
    {
        _builder = builder;

        For<NameNode>(Visit);
        For<LiteralNode>(Visit);
        For<GroupNode>(VisitNonTerminal, group => group.LeftSymbol == "<" && group.RightSymbol == ">");
        For<GroupNode>(VisitGroup, group => group.LeftSymbol == "(" && group.RightSymbol == ")");
        For<BlockNode>(Visit);
        For<PrefixOperatorNode>(Visit);
        For<PostfixOperatorNode>(VisitAsterisk, p => p.Operator == "*");
        For<PostfixOperatorNode>(VisitOptional, p => p.Operator == "?");
    }

    private void VisitOptional(PostfixOperatorNode obj)
    {
        if (obj.Expr is LiteralNode terminal)
        {
            AppendWithIndentation($"if (parser.IsMatch(\"{terminal.Value}\")) {{\n");
            _indentationLevel++;
            AppendWithIndentation($"parser.Consume();\n");
            _indentationLevel--;
            AppendWithIndentation("}\n");
        }
    }

    private void VisitGroup(GroupNode group)
    {
        Visit(group.Expr);
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

    private void Visit(PrefixOperatorNode prefix)
    {
        if (prefix.Operator.Text.Span == "_")
        {
        }
    }

    private void VisitAsterisk(PostfixOperatorNode postfix)
    {
        //Todo: extract name from expression
        //postfix.Expr.Accept(this);
    }

    private void Visit(NameNode name)
    {
        switch (name.Token.Text.ToString())
        {
            case "id":
                AppendWithIndentation("parser.Consume(PredefinedSymbols.Name)");
                break;
            case "expr":
                AppendWithIndentation("parser.ParseExpression()");
                break;
        }
    }

    private void VisitNonTerminal(GroupNode group)
    {
        switch (group.Expr)
        {
            case NameNode nonterminal:
                group.Expr.Accept(this);
                break;
            case BinaryOperatorNode bin when bin.Operator == ":":
                {
                    var name = GetName(bin.RightExpr);

                    if (name is null) return;
            
                    AppendWithIndentation($"var _{name.ToLower()} = ");
                    Names.Add(name);

                    bin.LeftExpr.Accept(this);
                    _builder.Append(";\n");
                    break;
                }
        }
    }

    private string? GetName(AstNode node)
    {
        if (node is NameNode name)
        {
            return name.Token.Text.ToString();
        }

        return null;
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

    public override string ToString()
    {
        return _builder.ToString();
    }
}
