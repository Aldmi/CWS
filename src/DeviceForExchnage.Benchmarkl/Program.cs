using BenchmarkDotNet.Running;

namespace DeviceForExchnage.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MiddleWareInDataBenchmark>();
        }
    }
}
