using CSharpFunctionalExtensions;
using InputDataModel.Base;
using InputDataModel.Base.InData;

namespace DeviceForExchange.MiddleWares.Invokes
{
    public interface ISupportMiddlewareInvoke<TIn>
    {
        Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData);
    }
}