using Silverfly.Parselets.Literals;

namespace Silverfly;

public partial class ParserDefinition
{
    public void AddArithmeticOperators()
    {
        Prefix("+");
        Prefix("-");
        Group("(", ")");
        InfixLeft("+", "Sum");
        InfixLeft("-", "Sum");
        InfixLeft("*", "Product");
        InfixLeft("/", "Product");
    }

    public void AddLogicalOperators()
    {
        Prefix("!");
        InfixLeft("&&", "Product");
        InfixLeft("||", "Sum");
    }

    public void AddBitOperators()
    {
        Prefix("~", "Prefix");
        InfixLeft("&", "Product");
        InfixLeft("|", "Sum");
        InfixLeft("<<", "Product");
        InfixLeft(">>", "Product");
    }

    public void AddCommonLiterals()
    {
        Register(PredefinedSymbols.Number, new NumberParselet());
        Register(PredefinedSymbols.Boolean, new BooleanLiteralParselet());
        Register(PredefinedSymbols.String, new StringLiteralParselet());
    }

    public void AddCommonAssignmentOperators()
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
}
