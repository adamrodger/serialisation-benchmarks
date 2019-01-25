using BenchmarkDotNet.Running;

namespace SerialiserBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SerialisationBenchmark<SimpleObject>>();
            BenchmarkRunner.Run<SerialisationBenchmark<ComplexObject>>();
        }
    }
}
