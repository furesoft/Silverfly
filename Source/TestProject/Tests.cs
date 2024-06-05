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

    [Test]
    public Task Dynamic_Should_Pass()
    {
        return Test("if true then 1 else 2");
    }

    [Test]
    public Task Dynamic_ifnt_Should_Pass()
    {
        return Test("ifnt true then 1");
    }

    [Test]
    public Task Dynamic_unless_Should_Pass()
    {
        return Test("unless true then 1");
    }

    public static Task Test(string source)
    {
        var result = Parser.Parse<TestParser>(source, useToplevelStatements: true);

        return Verify(result, settings);
    }
}
