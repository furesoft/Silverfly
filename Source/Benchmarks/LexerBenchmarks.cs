using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Silverfly;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[ShortRunJob]
[MemoryDiagnoser]
[Config(typeof(AntiVirusFriendlyConfig))]
public class LexerBenchmarks
{
    [Benchmark]
    public Token Number()
    {
        var lexer = new Lexer("42".AsMemory());

        return lexer.Next();
    }

    [Benchmark]
    public Token Symbol()
    {
        var lexer = new Lexer("+".AsMemory());

        return lexer.Next();
    }

    [Benchmark]
    public Token NumberWithWhitespace()
    {
        var lexer = new Lexer(" 42".AsMemory());

        return lexer.Next();
    }
}
