using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run(
            typeof(Program).Assembly, // all benchmarks from given assembly are going to be executed
            ManualConfig
                .Create(DefaultConfig.Instance)
                .With(ConfigOptions.JoinSummary)
                .With(ConfigOptions.DisableLogFile));
    }
}
