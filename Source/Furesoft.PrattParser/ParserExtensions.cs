using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets.Literals;

namespace Furesoft.PrattParser;

public static class ParserExtensions
{
    public static void AddArithmeticOperators(this Parser parser)
    {
        parser.Prefix("+", "Prefix");
        parser.Prefix("-", "Prefix");

        parser.Group("(", ")");

        parser.InfixLeft("+", "Sum");
        parser.InfixLeft("-", "Sum");
        parser.InfixLeft("*", "Product");
        parser.InfixLeft("/", "Product");
    }

    /// <summary>
    /// Return true if a given symbol was found and consumed
    /// </summary>
    /// <param name="parser"></param>
    /// <param name="optionalSymbol"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool Optional(this Parser parser, Symbol optionalSymbol)
    {
        if (!parser.IsMatch(optionalSymbol))
        {
            return false;
        }

        parser.Consume();

        return true;

    }

    public static void AddLogicalOperators(this Parser parser)
    {
        parser.Prefix("!", "Prefix");
        parser.InfixLeft("&&", "Product");
        parser.InfixLeft("||", "Sum");
    }

    public static void AddBitOperators(this Parser parser)
    {
        parser.Prefix("~", "Prefix");

        parser.InfixLeft("&", "Product");
        parser.InfixLeft("|", "Sum");

        parser.InfixLeft("<<", "Product");
        parser.InfixLeft(">>", "Product");
    }

    public static void AddCommonLiterals(this Parser parser)
    {
        parser.Register(PredefinedSymbols.Number, new NumberParselet());
        parser.Register(PredefinedSymbols.Boolean, new BooleanLiteralParselet());
        parser.Register(PredefinedSymbols.String, new StringLiteralParselet());
    }

    public static void AddCommonAssignmentOperators(this Parser parser)
    {
        parser.InfixLeft("=", "Assignment");
        parser.InfixLeft("+=", "Assignment");
        parser.InfixLeft("-=", "Assignment");
        parser.InfixLeft("*=", "Assignment");
        parser.InfixLeft("/=", "Assignment");

        parser.Prefix("++", "Prefix");
        parser.Prefix("--", "Prefix");

        parser.Postfix("--", "PostFix");
        parser.Postfix("++", "PostFix");
    }

    public static List<AstNode> ParseSeperated(this Parser parser, Symbol seperator, Symbol terminator, int bindingPower = 0)
    {
        var args = new List<AstNode>();

        if (parser.Match(terminator))
        {
            return [];
        }

        do
        {
            args.Add(parser.Parse(bindingPower));
        } while (parser.Match(seperator));

        parser.Consume(terminator);

        return args;
    }

    public static List<AstNode> ParseSeperated(this Parser parser, Symbol seperator, int bindingPower = 0, params Symbol[] terminators)
    {
        var args = new List<AstNode>();

        if (parser.Match(terminators))
        {
            return [];
        }

        do
        {
            args.Add(parser.Parse(bindingPower));
        } while (parser.Match(seperator));

        parser.Match(terminators);

        return args;
    }
}
