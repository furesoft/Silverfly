using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Silverfly;
using TestProject;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[ShortRunJob]
[MemoryDiagnoser]
[Config(typeof(AntiVirusFriendlyConfig))]
public class ParserBenchmarks
{
    private TestParser parser = new();

    [Benchmark]
    public TranslationUnit ParseNumber()
    {
        return parser.Parse("3.14");
    }

    [Benchmark]
    public TranslationUnit ParseAddition()
    {
        return parser.Parse("3.14 + 333");
    }

    [Benchmark]
    public TranslationUnit ParseCall()
    {
        return parser.Parse("3.14 + 333");
    }
}
