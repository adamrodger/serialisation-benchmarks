using BenchmarkDotNet.Running;

namespace SerialiserBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromTypes(new[]
            {
                typeof(SerialisationBenchmark<SimpleObject>),
                typeof(SerialisationBenchmark<ComplexObject>),
                typeof(DeserialisationBenchmark<SimpleObject>),
                typeof(DeserialisationBenchmark<ComplexObject>)
            })
            .RunAllJoined();
        }
    }
}
