using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Silverfly;
using Silverfly.Nodes;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[MemoryDiagnoser]
public class LexerBenchmarks
{
    [Benchmark]
    public Token Number()
    {
        var lexer = new Lexer("42");

        lexer.Next();

        return lexer.Next();
    }
}