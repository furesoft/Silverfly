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
        return new TestParser().Parse("3.14");
    }
}
