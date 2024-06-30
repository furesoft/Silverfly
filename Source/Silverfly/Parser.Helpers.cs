using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Parselets.Literals;

namespace Silverfly;

public partial class Parser
{
    protected void AddArithmeticOperators()
    {
        Prefix("+");
        Prefix("-");
        Group("(", ")");
        InfixLeft("+", "Sum");
        InfixLeft("-", "Sum");
        InfixLeft("*", "Product");
        InfixLeft("/", "Product");
    }

    protected void AddLogicalOperators()
    {
        Prefix("!");
        InfixLeft("&&", "Product");
        InfixLeft("||", "Sum");
    }

    protected void AddBitOperators()
    {
        Prefix("~", "Prefix");
        InfixLeft("&", "Product");
        InfixLeft("|", "Sum");
        InfixLeft("<<", "Product");
        InfixLeft(">>", "Product");
    }

    protected void AddCommonLiterals()
    {
        Register(PredefinedSymbols.Number, new NumberParselet());
        Register(PredefinedSymbols.Boolean, new BooleanLiteralParselet());
        Register(PredefinedSymbols.String, new StringLiteralParselet());
    }

    protected void AddCommonAssignmentOperators()
    {
        InfixLeft("=", "Assignment");
        InfixLeft("+=", "Assignment");
        InfixLeft("-=", "Assignment");
        InfixLeft("*=", "Assignment");
        InfixLeft("/=", "Assignment");
        Prefix("++");
        Prefix("--");
        Postfix("--");
        Postfix("++");
    }

    public ImmutableList<AstNode> ParseSeperated(Symbol seperator, Symbol terminator, int bindingPower = 0)
    {
        var args = new List<AstNode>();

        if (Match(terminator))
        {
            return [];
        }

        do
        {
            args.Add(Parse(bindingPower));
        } while (Match(seperator));

        Consume(terminator);

        return args.ToImmutableList();
    }

    public ImmutableList<AstNode> ParseList(Symbol terminator, int bindingPower = 0)
    {
        var args = new List<AstNode>();

        do
        {
            args.Add(Parse(bindingPower));
        } while (!IsMatch(terminator));

        Consume(terminator);

        return args.ToImmutableList();
    }

    public ImmutableList<AstNode> ParseSeperated(Symbol seperator, int bindingPower = 0, params Symbol[] terminators)
    {
        var args = new List<AstNode>();

        if (Match(terminators))
        {
            return [];
        }

        do
        {
            args.Add(Parse(bindingPower));
        } while (Match(seperator));

        Match(terminators);

        return args.ToImmutableList();
    }
}
