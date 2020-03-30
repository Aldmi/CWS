using BenchmarkDotNet.Attributes;
using Domain.InputDataModel.Autodictor.Model;

namespace DeviceForExchnage.Benchmark
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class MiddleWareInDataBenchmark
    {
        //[Benchmark(Description = "ParallelHandlers")]
        //public void ParallelDataHandlers()
        //{
        //    var middleWareInDataOption = InDataSourse.GetmiddleWareInDataOption();
        //    var inData = InDataSourse.GetData();

        //    var middleWareinData = new ParallelHandlers.MiddleWareInvoke<AdInputType>(middleWareInDataOption, null);
        //    middleWareinData.InputSet(inData).Wait();
        //}


        //[Benchmark(Description = "NotParallel")]
        //public void NotParallel()
        //{
        //    var middleWareInDataOption = InDataSourse.GetmiddleWareInDataOption();
        //    var inData = InDataSourse.GetData();

        //    var middleWareinData = new MiddleWareInvoke<AdInputType>(middleWareInDataOption, null);
        //    middleWareinData.InputSet(inData).Wait();
        //}


        //[Benchmark(Description = "ParallelData")]
        //public void ParallelData()
        //{
        //    var middleWareInDataOption = InDataSourse.GetmiddleWareInDataOption();
        //    var inData = InDataSourse.GetData();

        //    var middleWareinData = new ParallelData.MiddleWareInvoke<AdInputType>(middleWareInDataOption, null);
        //    middleWareinData.InputSet(inData).Wait();
        //}


        //[Benchmark(Description = "ParallelDataAndHandlers")]
        //public void ParallelDataAndHandlers()
        //{
        //    var middleWareInDataOption = InDataSourse.GetmiddleWareInDataOption();
        //    var inData = InDataSourse.GetData();

        //    var middleWareinData = new ParallelDataAndHandlers.MiddleWareInvoke<AdInputType>(middleWareInDataOption, null);
        //    middleWareinData.InputSet(inData).Wait();
        //}


    }
}