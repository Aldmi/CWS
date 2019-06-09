using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Xunit;
using Xunit.Abstractions;

namespace DeviceForExchnage.Test
{
    public class MiddleWareInDataTest
    {
        private readonly ITestOutputHelper output;

        public MiddleWareInDataTest(ITestOutputHelper output)
        {
            this.output = output;
        }



        [Fact]
        public void RunBenchmark()
        {
          var res=  BenchmarkRunner.Run<MiddleWareInDataBenchmark>();

            output.WriteLine("This is output from");
        }
    }



    public class MiddleWareInDataBenchmark
    {
        [Benchmark(Description = "Summ100")]
        public int Test100()
        {
            return Enumerable.Range(1, 100).Sum();
        }

        [Benchmark(Description = "Summ200")]
        public int Test200()
        {
            return Enumerable.Range(1, 200).Sum();
        }
    }


    


}