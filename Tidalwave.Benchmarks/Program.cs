using BenchmarkDotNet.Running;
using Tidalwave.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ContextBenchmarks>();
    }
}
