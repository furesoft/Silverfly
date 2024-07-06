using BenchmarkDotNet.Running;

namespace Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ParserBenchmarks>();
        BenchmarkRunner.Run<LexerBenchmarks>();
    }
}