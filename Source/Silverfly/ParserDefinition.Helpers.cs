using Silverfly.Parselets.Literals;

namespace Silverfly;

public partial class ParserDefinition
{
    /// <summary>
    /// Adds common arithmetic operators to the parser.
    /// </summary>
    /// <remarks>
    /// This method registers arithmetic operators and their precedence levels with the parser.
    /// The operators added include unary plus and minus, as well as binary operators for addition, subtraction, multiplication, and division.
    /// The precedence levels are set to ensure the correct order of operations in expressions.
    /// </remarks>
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

    /// <summary>
    /// Adds common logical operators to the parser.
    /// </summary>
    /// <remarks>
    /// This method registers logical operators and their precedence levels with the parser.
    /// The operators added include the logical NOT operator, as well as logical AND and OR operators.
    /// The precedence levels ensure that logical expressions are evaluated in the correct order.
    /// </remarks>
    public void AddLogicalOperators()
    {
        Prefix("!");
        InfixLeft("&&", "Product");
        InfixLeft("||", "Sum");
    }

    /// <summary>
    /// Adds common bitwise operators to the parser.
    /// </summary>
    /// <remarks>
    /// This method registers bitwise operators and their precedence levels with the parser.
    /// The operators added include bitwise NOT, AND, OR, and bit shifts (left and right).
    /// The precedence levels are configured to handle bitwise operations correctly in expressions.
    /// </remarks>
    public void AddBitOperators()
    {
        Prefix("~", "Prefix");
        InfixLeft("&", "Product");
        InfixLeft("|", "Sum");
        InfixLeft("<<", "Product");
        InfixLeft(">>", "Product");
    }

    /// <summary>
    /// Registers common literals with the parser.
    /// </summary>
    /// <remarks>
    /// This method adds common literal types to the parser, including numbers, boolean values, and strings.
    /// Each literal type is associated with a parselet that defines how to parse and handle that type of literal.
    /// </remarks>
    public void AddCommonLiterals()
    {
        Register(PredefinedSymbols.Number, new NumberParselet());
        Register(PredefinedSymbols.Boolean, new BooleanLiteralParselet());
        Register(PredefinedSymbols.String, new StringLiteralParselet());
    }

    /// <summary>
    /// Adds common assignment operators to the parser.
    /// </summary>
    /// <remarks>
    /// This method registers assignment operators and their precedence levels with the parser.
    /// The operators added include simple assignment and compound assignment operators (e.g., +=, -=).
    /// It also registers increment and decrement operators, both prefix and postfix.
    /// The precedence levels are set to handle assignment operations correctly in expressions.
    /// </remarks>
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
