using BenchmarkDotNet.Attributes;
using Silverfly;

namespace Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[Config(typeof(AntiVirusFriendlyConfig))]
public class LexerBenchmarks
{
    private LexerConfig config = new();

    [Benchmark]
    public Token Number()
    {
        var lexer = new Lexer("42".AsMemory(), config);

        return lexer.Next();
    }

    [Benchmark]
    public Token Symbol()
    {
        var lexer = new Lexer("+".AsMemory(), config);

        return lexer.Next();
    }

    [Benchmark]
    public Token NumberWithWhitespace()
    {
        var lexer = new Lexer(" 42".AsMemory(), config);

        return lexer.Next();
    }
}
