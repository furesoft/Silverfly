using System.Diagnostics;
using System.Text;
using Furesoft.PrattParser;

namespace Test;

public static class Program
{
    private static int _passed = 0;
    private static int _failed = 0;

    private static void Main(string[] args)
    {
        Test("0b11111111_11111111_11111111_11111111", uint.MaxValue.ToString());
        Test("0xff", $"255");
        Test("3.1e5; 1 + 2", $"{3.1e5}; (1 + 2)");
        
        Test("3.1e5", $"{3.1e5}");
        Test("not 5", "(not5)");
        
        Test("'hello'", "'hello'");
        Test("-521", "-521");
        Test("-521.33", "-521.33");
        
        Test("a(b)", "a(b)");
        
        Test("a -> b", "(a -> b)");
        Test("i++", "(i++)");
        //Test("!5\rhello", "(!5)");
        Test("not 5", "(not5)");

        // Function call.
        Test("a()", "a()");
        Test("a(b, c)", "a(b, c)");
        Test("a(b)(c)", "a(b)(c)");
        Test("a(b) + c(d)", "(a(b) + c(d))");
        Test("a(b ? c : d, e + f)", "a((b ? c : d), (e + f))");

        // Unary binding power.
        Test("~!-+a", "(~(!(-(+a))))");
        Test("a!!!", "(((a!)!)!)");

        // Unary and binary binding power.
        Test("-a * b", "((-a) * b)");
        Test("!a + b", "((!a) + b)");
        Test("~a ^ b", "((~a) ^ b)");
        Test("-a!", "(-(a!))");
        Test("!a!", "(!(a!))");

        // Binary binding power.
        Test("a = b + c * d ^ e - f / g", "(a = ((b + (c * (d ^ e))) - (f / g)))");

        // Binary associativity.
        Test("a = b = c", "(a = (b = c))");
        Test("a + b - c", "((a + b) - c)");
        Test("a * b / c", "((a * b) / c)");
        Test("a ^ b ^ c", "(a ^ (b ^ c))");

        // Conditional operator.
        Test("a ? b : c ? d : e", "(a ? b : (c ? d : e))");
        Test("a ? b ? c : d : e", "(a ? (b ? c : d) : e)");
        Test("a + b ? c * d : e / f", "((a + b) ? (c * d) : (e / f))");

        // Grouping.
        Test("a + (b + c) + d", "((a + (b + c)) + d)");
        Test("a ^ (b + c)", "(a ^ (b + c))");
        Test("(!a)!", "((!a)!)");

        // Show the results.
        if (_failed != 0)
        {
            Console.WriteLine("----");
        }

        Console.WriteLine("Passed: " + _passed);
        Console.WriteLine("Failed: " + _failed);

        if (Debugger.IsAttached)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public static void Test(string source, string expected)
    {
        var printVisitor = new PrintVisitor();

        var result = TestParser.Parse<TestParser>(source);

        var builder = new StringBuilder();

        builder.Append(result.Tree.Accept(printVisitor));

        var actual = builder.ToString();

        if (expected.Equals(actual))
        {
            _passed++;
        }
        else
        {
            _failed++;
            Console.WriteLine("[FAIL] Source: " + source);
            Console.WriteLine("     Expected: '" + expected + "'");
            Console.WriteLine("       Actual: '" + actual + "'");
        }

        if (result.Document.Messages.Any())
        {
            _failed++;
            Console.WriteLine("[FAIL] Source: " + source);
            Console.WriteLine("     Expected: " + expected);

            foreach (var message in result.Document.Messages)
            {
                Console.WriteLine($"        {message.Severity}: " + message);
            }
        }
    }
}
