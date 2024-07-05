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
            var node = Parse(bindingPower);

            if (node is not InvalidNode)
            {
                args.Add(node);
            }
        } while (Match(seperator));

        Match(terminator);

        return [.. args];
    }

    public ImmutableList<AstNode> ParseList(int bindingPower = 0, params Symbol[] terminators)
    {
        var args = new List<AstNode>();

        while (!IsMatch(terminators))
        {
            var node = Parse(bindingPower);

            if (node is not InvalidNode)
            {
                args.Add(node);
            }
        }

        Match(terminators);

        return [.. args];
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
            var node = Parse(bindingPower);

            if (node is not InvalidNode)
            {
                args.Add(node);
            }
        } while (Match(seperator));

        Match(terminators);

        return [.. args];
    }
}
