using BenchmarkDotNet.Attributes;
using Silverfly;

namespace Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
[Config(typeof(AntiVirusFriendlyConfig))]
public class LexerBenchmarks
{
    private readonly Lexer _lexer = new(new LexerConfig());

    [Benchmark]
    public Token Number()
    {
        _lexer.SetSource("42");

        return _lexer.Next();
    }

    [Benchmark]
    public Token Symbol()
    {
        _lexer.SetSource("+");

        return _lexer.Next();
    }

    [Benchmark]
    public Token NumberWithWhitespace()
    {
        _lexer.SetSource(" 42");

        return _lexer.Next();
    }
}
