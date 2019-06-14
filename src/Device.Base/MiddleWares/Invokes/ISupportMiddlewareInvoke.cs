using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using InputDataModel.Base;

namespace DeviceForExchange.MiddleWares.Invokes
{
    public interface ISupportMiddlewareInvoke<TIn>
    {
        Result<InputData<TIn>, ErrorMiddleWareInDataWrapper> HandleInvoke(InputData<TIn> inData);
    }
}