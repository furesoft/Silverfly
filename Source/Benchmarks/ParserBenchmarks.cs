using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Silverfly;
using TestProject;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class ParserBenchmarks
{
    [Benchmark]
    public TranslationUnit ParseNumber()
    {
        return Parser.Parse<TestParser>("3.14");
    }
}
