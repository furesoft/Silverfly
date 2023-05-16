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
        Test("a -> b", "(a -> b)");
        
        // Function call.
        Test("a()", "a()");
        Test("a(b)", "a(b)");
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
        var lexer = new Lexer(source);
        lexer.Ignore('\r');
        lexer.Ignore(' ');
        lexer.Ignore('\t');

        var a = lexer.Next();
        var op = lexer.Next();
        var b = lexer.Next();
        
        
        var parser = new TestParser(lexer);

        var printVisitor = new PrintVisitor();

        try
        {
            var result = parser.Parse();

            var builder = new StringBuilder();

            builder.Append(result.Accept(printVisitor));

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
        }
        catch (ParseException ex)
        {
            _failed++;
            Console.WriteLine("[FAIL] Source: " + source);
            Console.WriteLine("     Expected: " + expected);
            Console.WriteLine("        Error: " + ex.Message);
        }
    }
}
