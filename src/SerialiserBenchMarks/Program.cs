using BenchmarkDotNet.Running;

namespace SerialiserBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAllJoined();
            new SizeBenchmark().Print();
        }
    }
}
