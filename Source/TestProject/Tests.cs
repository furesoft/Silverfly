using System.Runtime.CompilerServices;
using Furesoft.PrattParser;
using Furesoft.PrattParser.Testing;

namespace TestProject;

[TestFixture]
public class Tests : SnapshotParserTestBase
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Init();
    }

    [Test]
    public Task Non_FloatingNumber_With_DecimalPoint_Should_Pass()
    {
        return Test("42.");
    }

    [Test]
    public Task EqualsOperatorPower_Should_Pass()
    {
        return Test("a = b = c");
    }

    [Test]
    public Task Integer_Should_Pass()
    {
        return Test("42");
    }

    [Test]
    public Task FunctionCall_Multiple_Should_Pass()
    {
        return Test("a(b, c)");
    }

    [Test]
    public Task FunctionCall_Multiple_Calling_Should_Pass()
    {
        return Test("a(b)(c)");
    }

    [Test]
    public Task Multiple_PostUnary_Should_Pass()
    {
        return Test("a!!!");
    }

    [Test]
    public Task Factor_Sequence_Should_Pass()
    {
        return Test("a * b / c");
    }

    [Test]
    public Task Preunary_Addition_Should_Pass()
    {
        return Test("!a + b");
    }

    [Test]
    public Task Preunary_Postunary_Should_Pass()
    {
        return Test("-a!");
    }

    [Test]
    public Task FunctionCalls_Addition_Should_Pass()
    {
        return Test("a(b) + c(d)");
    }

    [Test]
    public Task FunctionCall_Complex_Should_Pass()
    {
        return Test("a(b ? c : d, e + f)");
    }

    [Test]
    public Task NegativeInteger_Should_Pass()
    {
        return Test("-42");
    }

    [Test]
    public Task Boolean_Should_Pass()
    {
        return Test("true");
    }

    [Test]
    public Task Boolean_CaseInsensitive_Should_Pass()
    {
        return Test("TrUe");
    }

    [Test]
    public Task FloatingNumber_Should_Pass()
    {
        return Test("42.5");
    }

    [Test]
    public Task Number_E_Notation_Should_Pass()
    {
        return Test("3.1e5");
    }

    [Test]
    public Task Number_Binary_Should_Pass()
    {
        return Test("0b10101");
    }

    [Test]
    public Task Number_Seperator_Should_Pass()
    {
        return Test("1_000_000");
    }

    [Test]
    public Task BinaryNumber_Seperator_Should_Pass()
    {
        return Test("0b10_01");
    }

    [Test]
    public Task HexNumber_Seperator_Should_Pass()
    {
        return Test("0xF_F");
    }

    [Test]
    public Task Hex_Number_With_Comment_Should_Pass()
    {
        return Test("  /* i need something new*/0xff");
    }

    [Test]
    public Task Simple_FunctionCall_Should_Pass()
    {
        return Test("a(b)");
    }

    [Test]
    public Task Binary_Number_With_Comment_Should_Pass()
    {
        return Test("// this is a real long number\n0b11111111_11111111_11111111_11111111");
    }

    [Test]
    public Task Binary_Operator_Should_Pass()
    {
        return Test("a -> b");
    }

    [Test]
    public Task String_Should_Pass()
    {
        return Test("'hello'");
    }

    [Test]
    public Task String_Escape_Should_Pass()
    {
        return Test("'hel\tlo'");
    }

    [Test]
    public Task String_Unicode_Should_Pass()
    {
        return Test("'hel\\u00F61lo'");
    }

    [Test]
    public Task PostFix_Operator_Should_Pass()
    {
        return Test("i++");
    }

    [Test]
    public Task Function_Call_With_Ternary_Operator_Should_Pass()
    {
        return Test("a(b ? c : d, e + f)");
    }

    [Test]
    public Task Prefix_Operator_Sequence_Should_Pass()
    {
        return Test("~!-+a");
    }

    [Test]
    public Task Keyword_Not_Prefix_Should_Pass()
    {
        return Test("not 5");
    }

    [Test]
    public Task Multiply_With_Negation_Should_Pass()
    {
        return Test("-a * b");
    }

    [Test]
    public Task Binary_Binding_Power_Should_Pass()
    {
        return Test("a = b + c * d ^ e - f / g");
    }

    [Test]
    public Task Grouping_Should_Pass()
    {
        return Test("a + (b + c) + d");
    }

    [Test]
    public Task NegativeFloatingNumber_Should_Pass()
    {
        return Test("-42.5");
    }

    [Test]
    public Task Block_Should_Pass()
    {
        return Test("-42.5;13");
    }

    public static Task Test(string source)
    {
        var result = Parser.Parse<TestParser>(source, useStatementsAtToplevel: true);

        return Verify(result, settings);
    }
}
